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

        public async Task<IEnumerable<Employe>> GetAllEntitiesAsync()
        {
            return await _context.Employes
                .Include(e => e.EmployeProjets)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeDto>> GetAllEmp()
        {
            return await _context.Employes
                .GroupJoin(
                    _context.EmployeProjets,
                    e => e.EmployeId,
                    ep => ep.EmployeId,
                    (e, projets) => new EmployeDto
                    {
                        Id = e.EmployeId,
                        Nom = e.Nom,
                        Prenom = e.Prenom,
                        Email = e.Email,
                        Role = e.Role,
                        NombreDeProjet = projets.Count()
                    })
                .ToListAsync();
        }

        public async Task<Employe?> GetByUsernameAsync(string username)
        {
            var normalized = username?.Trim().ToLower();
            return await _context.Employes
                .FirstOrDefaultAsync(e => e.Username.ToLower() == normalized);
        }

        public async Task<Employe?> GetByEmailAsync(string email)
        {
            var normalized = NormalizeEmail(email);
            return await _context.Employes
                .Include(e => e.EmployeRoles)
                .FirstOrDefaultAsync(e => e.Email.ToLower() == normalized);
        }

        public async Task<Employe?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Employes
                .FirstOrDefaultAsync(e => e.EmployeId == id);
        }

        public async Task<Employe?> GetWithProjetsByIdAsync(Guid id)
        {
            return await _context.Employes
                .Include(e => e.EmployeProjets)
                    .ThenInclude(ep => ep.Projet)
                .FirstOrDefaultAsync(e => e.EmployeId == id);
        }

        public async Task AddAsync(Employe employe)
        {
            if (employe == null)
                throw new ArgumentNullException(nameof(employe));

            await _context.Employes.AddAsync(employe);
        }

        public async Task UpdateEntityAsync(Employe employe)
        {
            if (employe == null)
                throw new ArgumentNullException(nameof(employe));

            _context.Employes.Update(employe);
            await _context.SaveChangesAsync();
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

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var normalized = NormalizeEmail(email);
            return await _context.Employes
                .AnyAsync(e => e.Email.ToLower() == normalized);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private static string NormalizeEmail(string email)
        {
            return email?.Trim().ToLower() ?? string.Empty;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _context.Employes.AnyAsync(e => e.EmployeId == id);
        }
    }
}

