using GestionDisponibilite.Data;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionDisponibilite.Repository
{
    public class EmployeProjetRepository : IEmployeProjetRepository
    {
        private readonly AppDbContext _context;

        public EmployeProjetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeProjetDetailDto>> GetAllAsync()
        {
            var entities = await _context.EmployeProjets
                .Include(ep => ep.Employe)
                .Include(ep => ep.Projet)
                .ToListAsync();

            return entities.Adapt<List<EmployeProjetDetailDto>>();
        }

        public async Task<EmployeProjetDetailDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.EmployeProjets
                .Include(ep => ep.Employe)
                .Include(ep => ep.Projet)
                .FirstOrDefaultAsync(ep => ep.Id == id);

            return entity?.Adapt<EmployeProjetDetailDto>();
        }

        public async Task<EmployeProjetDto> CreateAsync(CreateEmployeProjetDto dto)
        {
            var exists = await _context.EmployeProjets
                .AnyAsync(ep => ep.EmployeId == dto.EmployeId && ep.ProjetId == dto.ProjetId);

            if (exists)
                throw new InvalidOperationException("This employee is already assigned to this project.");

            var entity = dto.Adapt<EmployeProjet>();
            _context.EmployeProjets.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Adapt<EmployeProjetDto>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.EmployeProjets.FindAsync(id);
            if (entity is null) return false;

            _context.EmployeProjets.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<EmployeProjet?> GetByEmployeAndProjetAsync(Guid employeId, Guid projetId)
        {
            return _context.EmployeProjets
                .FirstOrDefaultAsync(ep => ep.EmployeId == employeId && ep.ProjetId == projetId);
        }

        public async Task<EmployeProjet> CreateRawAsync(EmployeProjet entity)
        {
            _context.EmployeProjets.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
