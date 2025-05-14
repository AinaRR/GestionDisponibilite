using GestionDisponibilite.Data;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace GestionDisponibilite.Service
{
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository _employeRepository;
        private readonly IProjetRepository _projetRepository;
        private readonly IEmployeProjetRepository _employeProjetRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<EmployeService> _logger;

        // Allowed roles (optional: move to config or constants)
        private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "User",
        "Admin"
    };

        public EmployeService(
            IEmployeRepository employeRepository,
            IProjetRepository projetRepository,
            IEmployeProjetRepository employeProjetRepository,
            IPasswordHasher passwordHasher,
            ILogger<EmployeService> logger)
        {
            _employeRepository = employeRepository;
            _projetRepository = projetRepository;
            _employeProjetRepository = employeProjetRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        // ==================== Link employe to project ====================
        public async Task<EmployeProjetDto> LinkEmployeToProjetAsync(Guid employeId, Guid projetId)
        {
            var employe = await _employeRepository.GetEntityByIdAsync(employeId)
                          ?? throw new DomainException($"No employee found with ID {employeId}");

            var projet = await _projetRepository.GetEntityByIdAsync(projetId)
                         ?? throw new DomainException($"No project found with ID {projetId}");

            var existing = await _employeProjetRepository.GetByEmployeAndProjetAsync(employeId, projetId);
            if (existing != null)
                throw new DomainException("This employee is already linked to the project.");

            await using var transaction = await _employeProjetRepository.BeginTransactionAsync();

            try
            {
                var entity = new EmployeProjet
                {
                    Id = Guid.NewGuid(),
                    EmployeId = employeId,
                    ProjetId = projetId
                };

                var created = await _employeProjetRepository.CreateRawAsync(entity);
                await transaction.CommitAsync();

                return created.Adapt<EmployeProjetDto>();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to link employee to project");
                throw;
            }
        }

        // ==================== Create employe (admin use) ====================
        public async Task<EmployeDto> CreateEmployeAsync(CreateEmployeDto dto)
        {
            await EnsureEmailIsUniqueAsync(dto.Email);
            var role = ValidateOrDefaultRole(dto.Role);
            var hashedPassword = HashPasswordOrThrow(dto.Password);

            var employe = dto.Adapt<Employe>();
            employe.EmployeID = Guid.NewGuid();
            employe.PasswordHash = hashedPassword;
            employe.Role = role;

            await _employeRepository.AddAsync(employe);
            await _employeRepository.SaveChangesAsync();

            return employe.Adapt<EmployeDto>();
        }

        // ==================== Register employe (public use) ====================
        public async Task<EmployeDto> RegisterEmployeAsync(RegisterEmployeDto dto)
        {
            await EnsureEmailIsUniqueAsync(dto.Email);
            var hashedPassword = HashPasswordOrThrow(dto.Password);

            var employe = dto.Adapt<Employe>();
            employe.EmployeID = Guid.NewGuid();
            employe.PasswordHash = hashedPassword;
            employe.Role = "User";

            await _employeRepository.AddAsync(employe);
            await _employeRepository.SaveChangesAsync();

            return employe.Adapt<EmployeDto>();
        }

        // ==================== Helper Methods ====================
        private string HashPasswordOrThrow(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password is required.");
            return _passwordHasher.HashPassword(password);
        }

        private string ValidateOrDefaultRole(string? inputRole)
        {
            var role = string.IsNullOrWhiteSpace(inputRole) ? "User" : inputRole.Trim();
            if (!AllowedRoles.Contains(role))
                throw new DomainException($"Invalid role: {role}. Allowed roles: {string.Join(", ", AllowedRoles)}");
            return role;
        }

        private async Task EnsureEmailIsUniqueAsync(string email)
        {
            if (await _employeRepository.ExistsByEmailAsync(email))
                throw new DomainException("An employee with this email already exists.");
        }
    }
}
