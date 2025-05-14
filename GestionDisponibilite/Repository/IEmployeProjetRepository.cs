using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;

namespace GestionDisponibilite.Repository
{
    public interface IEmployeProjetRepository
    {
        // Read operations
        Task<IEnumerable<EmployeProjetDetailDto>> GetAllAsync();
        Task<EmployeProjetDetailDto?> GetByIdAsync(Guid id);
        Task<EmployeProjet?> GetByEmployeAndProjetAsync(Guid employeId, Guid projetId);
        // Write operations
        Task<EmployeProjetDto> CreateAsync(CreateEmployeProjetDto dto);
        Task<EmployeProjet> CreateRawAsync(EmployeProjet entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
