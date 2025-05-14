using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;

namespace GestionDisponibilite.Repository
{
    public interface IProjetRepository
    {
        Task<IEnumerable<ProjetDto>> GetAll(); // Retrieves all projects
        Task<ProjetDto?> GetById(Guid id); // Retrieves a project by ID
        Task<ProjetDto> Add(CreateProjetDto dto); // Creates a project
        Task<ProjetDto?> Update(Guid id, UpdateProjetDto dto); // Updates a project
        Task<bool> Delete(Guid id); // Deletes a project
        Task<Projet?> GetEntityByIdAsync(Guid id);
    }
}
