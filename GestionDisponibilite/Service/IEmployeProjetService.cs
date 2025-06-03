using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Service
{
    public interface IEmployeProjetService
    {
        /// <summary>
        /// Récupère toutes les associations employé-projet.
        /// </summary>
        Task<IEnumerable<EmployeProjetDetailDto>> GetAllAsync();

        /// <summary>
        /// Récupère une association employé-projet par son identifiant unique.
        /// </summary>
        Task<EmployeProjetDetailDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Crée une nouvelle association employé-projet.
        /// </summary>
        Task<EmployeProjetDto> CreateAsync(CreateEmployeProjetDto dto);

        /// <summary>
        /// Supprime une association employé-projet par ID.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);
    }
}
