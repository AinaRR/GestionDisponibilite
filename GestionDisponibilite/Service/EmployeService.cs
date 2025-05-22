using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Options;
using GestionDisponibilite.Repository;
using GestionDisponibilite.RoleUser;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDisponibilite.Service
{
    /// <summary>
    /// Service applicatif principal pour la gestion des employés.
    /// </summary>
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository _employeRepository;
        private readonly IProjetRepository _projetRepository;
        private readonly IEmployeProjetRepository _employeProjetRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly PasswordPolicyOptions _pwdPolicy;
        private readonly ILogger<EmployeService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            Roles.Admin,
            Roles.User
        };

        public EmployeService(
            IEmployeRepository employeRepository,
            IProjetRepository projetRepository,
            IEmployeProjetRepository employeProjetRepository,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            IOptions<PasswordPolicyOptions> pwdPolicyOptions,
            ILogger<EmployeService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _employeRepository = employeRepository ?? throw new ArgumentNullException(nameof(employeRepository));
            _projetRepository = projetRepository ?? throw new ArgumentNullException(nameof(projetRepository));
            _employeProjetRepository = employeProjetRepository ?? throw new ArgumentNullException(nameof(employeProjetRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pwdPolicy = pwdPolicyOptions?.Value ?? throw new ArgumentNullException(nameof(pwdPolicyOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        #region Public API

        public async Task<EmployeProjetDto> LinkEmployeToProjetAsync(Guid employeId, Guid projetId)
        {
            var employe = await _employeRepository.GetEntityByIdAsync(employeId)
                ?? throw new DomainException($"Aucun employé trouvé avec l'ID {employeId}.");

            var projet = await _projetRepository.GetEntityByIdAsync(projetId)
                ?? throw new DomainException($"Aucun projet trouvé avec l'ID {projetId}.");

            if (await _employeProjetRepository.GetByEmployeAndProjetAsync(employeId, projetId) is not null)
                throw new DomainException("L'employé est déjà associé à ce projet.");

            await using var tx = await _employeProjetRepository.BeginTransactionAsync();
            try
            {
                var entity = new EmployeProjet
                {
                    Id = Guid.NewGuid(),
                    EmployeId = employeId,
                    ProjetId = projetId
                };
                var created = await _employeProjetRepository.CreateRawAsync(entity);
                await tx.CommitAsync();
                return created.Adapt<EmployeProjetDto>();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, "Échec de l'association de l'employé au projet.");
                throw new DomainException("Une erreur s'est produite lors de l'association.", ex);
            }
        }

        public async Task<EmployeDto> CreateEmployeAsync(CreateEmployeDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            var role = ValidateOrDefaultRole(dto.Role);
            var employe = dto.Adapt<Employe>();
            return await CreateAndSaveEmployeAsync(employe, dto.Password, role);
        }

        public async Task<EmployeDto> RegisterAsync(RegisterEmployeDto dto, bool isAdmin = false)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            var role = isAdmin ? Roles.Admin : Roles.User;
            var employe = dto.Adapt<Employe>();
            return await CreateAndSaveEmployeAsync(employe, dto.Password, role);
        }

        #endregion

        #region Helpers

        private async Task<EmployeDto> CreateAndSaveEmployeAsync(Employe employe, string password, string role)
        {
            await EnsureEmailIsUniqueAsync(employe.Email);
            employe.EmployeId = Guid.NewGuid();
            employe.PasswordHash = HashPasswordOrThrow(password);
            employe.Role = role;

            await _employeRepository.AddAsync(employe);
            await _employeRepository.SaveChangesAsync();

            return employe.Adapt<EmployeDto>();
        }

        private string HashPasswordOrThrow(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Le mot de passe est requis.");
            if (password.Length < _pwdPolicy.MinLength)
                throw new DomainException($"Le mot de passe doit contenir au moins {_pwdPolicy.MinLength} caractères.");
            if (_pwdPolicy.RequireUppercase && !password.Any(char.IsUpper))
                throw new DomainException("Le mot de passe doit contenir au moins une lettre majuscule.");
            if (_pwdPolicy.RequireLowercase && !password.Any(char.IsLower))
                throw new DomainException("Le mot de passe doit contenir au moins une lettre minuscule.");
            if (_pwdPolicy.RequireDigit && !password.Any(char.IsDigit))
                throw new DomainException("Le mot de passe doit contenir au moins un chiffre.");
            if (_pwdPolicy.RequireNonAlphanumeric && !password.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DomainException("Le mot de passe doit contenir au moins un caractère spécial.");
            return _passwordHasher.HashPassword(password);
        }

        private string ValidateOrDefaultRole(string? inputRole)
        {
            var role = string.IsNullOrWhiteSpace(inputRole) ? Roles.User : inputRole.Trim();
            if (!AllowedRoles.Contains(role))
                throw new DomainException($"Rôle invalide : {role}. Rôles valides : {string.Join(", ", AllowedRoles)}");
            return role;
        }

        private async Task EnsureEmailIsUniqueAsync(string email)
        {
            if (await _employeRepository.ExistsByEmailAsync(email))
                throw new DomainException("Un employé avec cet email existe déjà.");
        }

        /// <summary>
        /// Récupère l'ID de l'utilisateur courant à partir des claims JWT.
        /// </summary>
        private Guid GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var guid)
                ? guid
                : throw new DomainException("Utilisateur non authentifié ou identifiant invalide.");
        }

        #endregion
    }
}



