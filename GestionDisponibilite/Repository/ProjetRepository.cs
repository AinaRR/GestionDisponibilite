using GestionDisponibilite.Data;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GestionDisponibilite.Repository
{
    public class ProjetRepository : IProjetRepository
    {
        private readonly AppDbContext _context;

        public ProjetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjetDto>> GetAll()
        {
            var projets = await _context.Projets.ToListAsync();
            var projetDtos = projets.Select(p => new ProjetDto
            {
                ProjetId = p.ProjetId,
                NomDuProjet = p.NomDuProjet,
                Client = p.Client,
                FinProjet = p.FinProjet,
                NombreDeEmploye = _context.EmployeProjets.Count(ep => ep.ProjetId == p.ProjetId) // Query count separately
            }).ToList();

            return projetDtos;
        }

        public async Task<ProjetDto?> GetById(Guid id)
        {
            return await _context.Projets
                .Where(p => p.ProjetId == id)
                .Select(p => new ProjetDto
                {
                    ProjetId = p.ProjetId,
                    NomDuProjet = p.NomDuProjet,
                    Client = p.Client,
                    FinProjet = p.FinProjet,
                    NombreDeEmploye = p.EmployeProjets.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ProjetDto> Add(CreateProjetDto dto)
        {
            var projet = dto.Adapt<Projet>();
            projet.ProjetId = Guid.NewGuid();

            _context.Projets.Add(projet);
            await _context.SaveChangesAsync();

            // Map and return the ProjetDto after saving
            return projet.Adapt<ProjetDto>();
        }

        public async Task<ProjetDto?> Update(Guid id, UpdateProjetDto dto)
        {
            var projet = await _context.Projets.FirstOrDefaultAsync(p => p.ProjetId == id);
            if (projet is null) return null;

            dto.Adapt(projet);
            await _context.SaveChangesAsync();

            return new ProjetDto
            {
                ProjetId = projet.ProjetId,
                NomDuProjet = projet.NomDuProjet,
                Client = projet.Client,
                FinProjet = projet.FinProjet,
                NombreDeEmploye = projet.EmployeProjets.Count
            };
        }

        public async Task<bool> Delete(Guid id)
        {
            var projet = await _context.Projets.FirstOrDefaultAsync(p => p.ProjetId == id);
            if (projet is null) return false;

            _context.Projets.Remove(projet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Projet?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Projets.FirstOrDefaultAsync(p => p.ProjetId == id);
        }
    }
}
