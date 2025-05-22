using GestionDisponibilite.Data;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionDisponibilite.Repository
{
    public class EmployeRepository : IEmployeRepository
    {
        private readonly AppDbContext _context;

        public EmployeRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Récupère tous les employés avec leurs projets liés, sans suivi EF.
        /// </summary>
        public async Task<IEnumerable<EmployeDto>> GetAllAsync()
        {
            return await _context.Employes
                .AsNoTracking()
                .Include(e => e.EmployeProjets)
                .ProjectToType<EmployeDto>()
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un employé par son ID, sans suivi EF.
        /// </summary>
        public async Task<EmployeDto?> GetByIdAsync(Guid id)
        {
            var employe = await _context.Employes
                .AsNoTracking()
                .Include(e => e.EmployeProjets)
                .FirstOrDefaultAsync(e => e.EmployeId == id);

            return employe?.Adapt<EmployeDto>();
        }

        /// <summary>
        /// Récupère une entité employé par son nom d'utilisateur (inclut Role).
        /// </summary>
        public async Task<Employe?> GetByUsernameAsync(string username)
        {
            var normalized = username.Trim().ToLower();
            return await _context.Employes.FirstOrDefaultAsync(
                    e => e.Username.ToLower() == normalized);
        }

        /// <summary>
        /// Récupère une entité employé par son e‑mail (inclut les rôles).
        /// </summary>
        public async Task<Employe?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var normalized = email.Trim().ToLower();
            return await _context.Employes
                .Include(e => e.EmployeRoles)
                .FirstOrDefaultAsync(e => e.Email.ToLower() == normalized);
        }

        /// <summary>
        /// Crée un nouvel employé à partir du DTO.
        /// </summary>
        /// <exception cref="ArgumentException">Si un employé avec le même email existe déjà.</exception>
        public async Task<EmployeDto> CreateAsync(CreateEmployeDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var normalizedEmail = NormalizeEmail(dto.Email);

            var exists = await _context.Employes
                .AnyAsync(e => e.Email.ToLower() == normalizedEmail);

            if (exists)
                throw new ArgumentException("Un employé avec cet email existe déjà.");

            var entity = dto.Adapt<Employe>();
            entity.EmployeId = Guid.NewGuid();
            entity.Email = normalizedEmail;

            _context.Employes.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<EmployeDto>();
        }

        /// <summary>
        /// Met à jour un employé existant à partir du DTO.
        /// </summary>
        /// <returns>EmployeDto mis à jour ou null si non trouvé.</returns>
        /// <exception cref="ArgumentException">Si un autre employé avec le même email existe.</exception>
        public async Task<EmployeDto?> UpdateAsync(Guid id, UpdateEmployeDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _context.Employes.FindAsync(id);
            if (entity == null) return null;

            var normalizedEmail = NormalizeEmail(dto.Email);
            var emailTaken = await _context.Employes
                .AnyAsync(e => e.EmployeId != id && e.Email.ToLower() == normalizedEmail);

            if (emailTaken)
                throw new ArgumentException("Un autre employé avec cet email existe déjà.");

            dto.Adapt(entity);
            entity.Email = normalizedEmail;

            await _context.SaveChangesAsync();

            return entity.Adapt<EmployeDto>();
        }

        /// <summary>
        /// Supprime un employé par son ID.
        /// </summary>
        /// <returns>True si supprimé, false si non trouvé.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Employes.FindAsync(id);
            if (entity == null) return false;

            _context.Employes.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Récupère une entité employé sans tracking par ID.
        /// </summary>
        public Task<Employe?> GetEntityByIdAsync(Guid id) =>
            _context.Employes.FindAsync(id).AsTask();

        /// <summary>
        /// Met à jour une entité employé existante.
        /// </summary>
        public async Task UpdateEntityAsync(Employe employe)
        {
            if (employe == null)
                throw new ArgumentNullException(nameof(employe));

            _context.Employes.Update(employe);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Vérifie si un email est déjà utilisé par un employé.
        /// </summary>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var normalizedEmail = NormalizeEmail(email);
            return await _context.Employes
                .AnyAsync(e => e.Email.ToLower() == normalizedEmail);
        }

        /// <summary>
        /// Ajoute un nouvel employé sans sauvegarder immédiatement.
        /// </summary>
        public async Task AddAsync(Employe employe)
        {
            if (employe == null)
                throw new ArgumentNullException(nameof(employe));

            await _context.Employes.AddAsync(employe);
        }

        /// <summary>
        /// Sauvegarde les changements dans le contexte.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Normalise un email (trim + minuscules).
        /// </summary>
        private static string NormalizeEmail(string email)
        {
            return email?.Trim().ToLower() ?? string.Empty;
        }
    }
}

