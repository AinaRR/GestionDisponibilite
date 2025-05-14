using GestionDisponibilite.Data;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GestionDisponibilite.Repository
{
    public class EmployeRepository : IEmployeRepository
    {
        private readonly AppDbContext _context;
        public EmployeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeDto>> GetAllAsync()
        {
            return await _context.Employes
                .AsNoTracking()
                .Include(e => e.EmployeProjets)
                .ProjectToType<EmployeDto>()
                .ToListAsync();
        }

        public async Task<EmployeDto?> GetByIdAsync(Guid id)
        {
            var employe = await _context.Employes
                .AsNoTracking()
                .Include(e => e.EmployeProjets)
                .FirstOrDefaultAsync(e => e.EmployeID == id);

            return employe?.Adapt<EmployeDto>();
        }

        public async Task<EmployeDto> CreateAsync(CreateEmployeDto dto)
        {
            var emailToCheck = dto.Email.ToLower();

            var existingEmploye = await _context.Employes
                .FirstOrDefaultAsync(e => e.Email.ToLower() == emailToCheck);

            if (existingEmploye != null)
            {
                throw new ArgumentException("An employe with this email already exists.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Password is required.");
            }

            var entity = dto.Adapt<Employe>();
            if (entity == null)
            {
                throw new InvalidOperationException("Failed to map CreateEmployeDto to Employe.");
            }

            entity.EmployeID = Guid.NewGuid();
            entity.SetPassword(dto.Password);

            _context.Employes.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<EmployeDto>();
        }



        public async Task<EmployeDto?> UpdateAsync(Guid id, UpdateEmployeDto dto)
        {
            var entity = await _context.Employes.FindAsync(id);
            if (entity is null) return null;

            var emailToCheck = dto.Email.ToLower();

            var existingEmail = await _context.Employes
                .FirstOrDefaultAsync(e => e.EmployeID != id && e.Email.ToLower() == emailToCheck);

            if (existingEmail != null)
            {
                throw new ArgumentException("An employe with this email already exists.");
            }

            dto.Adapt(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<EmployeDto>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Employes.FindAsync(id);
            if (entity is null) return false;

            _context.Employes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<Employe?> GetEntityByIdAsync(Guid id) =>
            _context.Employes.FindAsync(id).AsTask();

        public async Task UpdateEntityAsync(Employe employe)
        {
            _context.Employes.Update(employe);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Employes.AnyAsync(e => e.Email == email);
        }
    }
}
