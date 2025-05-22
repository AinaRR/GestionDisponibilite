using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Options;
using GestionDisponibilite.Repository;
using GestionDisponibilite.RoleUser;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PasswordPolicyOptions _pwdPolicy;
        private readonly ILogger<EmployeService> _logger;
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
        IOptions<PasswordPolicyOptions> pwdPolicyOptions,
        ILogger<EmployeService> logger)
        {
            _employeRepository = employeRepository ?? throw new ArgumentNullException(nameof(employeRepository));
            _projetRepository = projetRepository ?? throw new ArgumentNullException(nameof(projetRepository));
            _employeProjetRepository = employeProjetRepository ?? throw new ArgumentNullException(nameof(employeProjetRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _pwdPolicy = pwdPolicyOptions?.Value ?? throw new ArgumentNullException(nameof(pwdPolicyOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #region Public API 
        /// <inheritdoc /> 
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
        /// <inheritdoc /> 
        public async Task<EmployeDto> CreateEmployeAsync(CreateEmployeDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            await EnsureEmailIsUniqueAsync(dto.Email);
            var role = ValidateOrDefaultRole(dto.Role);
            var hashedPassword = HashPasswordOrThrow(dto.Password);
            var employe = dto.Adapt<Employe>();
            employe.EmployeId = Guid.NewGuid();
            employe.PasswordHash = hashedPassword;
            employe.Role = role;
            await _employeRepository.AddAsync(employe);
            await _employeRepository.SaveChangesAsync();
            return employe.Adapt<EmployeDto>();
        }
        /// <inheritdoc /> 
        public async Task<EmployeDto> RegisterAsync(RegisterEmployeDto dto, bool isAdmin = false)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            await EnsureEmailIsUniqueAsync(dto.Email);
            var role = isAdmin && !string.IsNullOrWhiteSpace(dto.Role)
            ? ValidateOrDefaultRole(dto.Role)
            : Roles.User;
            var employe = dto.Adapt<Employe>();
            employe.EmployeId = Guid.NewGuid();
            employe.PasswordHash = HashPasswordOrThrow(dto.Password);
            employe.Role = role;
            await _employeRepository.AddAsync(employe);
            await _employeRepository.SaveChangesAsync();
            return employe.Adapt<EmployeDto>();
        }
        #endregion
        #region Private Helpers 
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
        #endregion
    }
}



