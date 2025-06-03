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
            Roles.User,
            Roles.SuperAdmin
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
            _employeRepository = employeRepository;
            _projetRepository = projetRepository;
            _employeProjetRepository = employeProjetRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _pwdPolicy = pwdPolicyOptions?.Value ?? throw new ArgumentNullException(nameof(pwdPolicyOptions));
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeProjetDto> LinkEmployeToProjetAsync(Guid employeId, Guid projetId)
        {
            var employe = await _employeRepository.GetWithProjetsByIdAsync(employeId)
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

                employe.EmployeProjets.Add(entity);

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
            var role = ValidateOrDefaultRole(dto.Role);
            var employe = _mapper.Map<Employe>(dto);
            return await CreateAndSaveEmployeAsync(employe, dto.Password, role);
        }

        public async Task<EmployeDto> RegisterAsync(RegisterEmployeDto dto, bool isAdmin = false)
        {
            var role = isAdmin ? Roles.Admin : Roles.User;
            var employe = _mapper.Map<Employe>(dto);
            return await CreateAndSaveEmployeAsync(employe, dto.Password, role);
        }

        public async Task<IEnumerable<EmployeDto>> GetAllAsync()
        {
            var employes = await _employeRepository.GetAllEntitiesAsync();
            return _mapper.Map<IEnumerable<EmployeDto>>(employes);
        }

        public async Task<EmployeDto?> GetByIdAsync(Guid id)
        {
            var employe = await _employeRepository.GetWithProjetsByIdAsync(id);
            return employe == null ? null : _mapper.Map<EmployeDto>(employe);
        }

        public async Task<IEnumerable<ProjetDto>> GetProjetsByEmployeIdAsync(Guid employeId)
        {
            var employe = await _employeRepository.GetWithProjetsByIdAsync(employeId);
            if (employe == null)
                throw new DomainException($"Aucun employé trouvé avec l'ID {employeId}.");

            var projets = employe.EmployeProjets
                .Where(ep => ep.Projet != null)
                .Select(ep => ep.Projet)
                .ToList();

            return _mapper.Map<IEnumerable<ProjetDto>>(projets);
        }
        public async Task<EmployeDto?> UpdateEmployeAsync(Guid id, UpdateEmployeDto dto)
        {
            var entity = await _employeRepository.GetEntityByIdAsync(id);
            if (entity == null) return null;

            var newEmail = dto.Email?.Trim().ToLowerInvariant();
            if (!string.Equals(entity.Email?.ToLowerInvariant(), newEmail, StringComparison.OrdinalIgnoreCase))
            {
                await EnsureEmailIsUniqueAsync(newEmail);
                entity.Email = newEmail;
            }

            _mapper.Map(dto, entity);
            await _employeRepository.UpdateEntityAsync(entity);
            return _mapper.Map<EmployeDto>(entity);
        }

        public async Task<bool> DeleteEmployeAsync(Guid id)
        {
            return await _employeRepository.DeleteAsync(id);
        }

        #region Helpers

        private async Task<EmployeDto> CreateAndSaveEmployeAsync(Employe employe, string password, string role)
        {
            await EnsureEmailIsUniqueAsync(employe.Email);
            employe.EmployeId = Guid.NewGuid();
            employe.PasswordHash = HashPasswordOrThrow(password);
            employe.Role = role;

            await _employeRepository.AddAsync(employe);
            await _employeRepository.SaveChangesAsync();

            return _mapper.Map<EmployeDto>(employe);
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

        private async Task EnsureEmailIsUniqueAsync(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email requis.");
            if (await _employeRepository.ExistsByEmailAsync(email))
                throw new DomainException("Un employé avec cet email existe déjà.");
        }

        private string ValidateOrDefaultRole(string? inputRole)
        {
            var role = string.IsNullOrWhiteSpace(inputRole) ? Roles.User : inputRole.Trim();
            if (!AllowedRoles.Contains(role))
                throw new DomainException($"Rôle invalide : {role}. Rôles valides : {string.Join(", ", AllowedRoles)}");
            return role;
        }

        private Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var id)
                ? id
                : throw new DomainException("Utilisateur non authentifié.");
        }

        #endregion
    }
}



