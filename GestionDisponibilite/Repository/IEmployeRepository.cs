using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionDisponibilite.Repository
{
    public interface IEmployeRepository
    {
        Task<IEnumerable<Employe>> GetAllEntitiesAsync();
        Task<IEnumerable<EmployeDto>> GetAllEmp();
        /// <summary>
        /// Récupère une entité employé par son nom d'utilisateur.
        /// </summary>
        Task<Employe?> GetByUsernameAsync(string username);

        /// <summary>
        /// Récupère une entité employé par son e-mail (incluant les rôles).
        /// </summary>
        Task<Employe?> GetByEmailAsync(string email);

        /// <summary>
        /// Récupère une entité employé par son identifiant (sans tracking).
        /// </summary>
        Task<Employe?> GetEntityByIdAsync(Guid id);

        /// <summary>
        /// Ajoute un nouvel employé (sans sauvegarder immédiatement).
        /// </summary>
        Task AddAsync(Employe employe);

        /// <summary>
        /// Met à jour une entité employé existante.
        /// </summary>
        Task UpdateEntityAsync(Employe employe);

        /// <summary>
        /// Supprime un employé par son identifiant.
        /// </summary>
        /// <returns>True si supprimé, false si non trouvé.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Vérifie si un e-mail est déjà utilisé par un employé.
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);

        Task<Employe?> GetWithProjetsByIdAsync(Guid id);

        /// <summary>
        /// Sauvegarde les changements effectués dans le contexte.
        /// </summary>
        Task SaveChangesAsync();
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
