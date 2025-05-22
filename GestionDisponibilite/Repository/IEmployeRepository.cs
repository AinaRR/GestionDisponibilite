using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionDisponibilite.Repository
{
    public interface IEmployeRepository
    {
        Task<IEnumerable<EmployeDto>> GetAllAsync();
        Task<EmployeDto?> GetByIdAsync(Guid id);
        Task<Employe?> GetByUsernameAsync(string username);
        Task<Employe?> GetByEmailAsync(string email);
        Task<EmployeDto> CreateAsync(CreateEmployeDto dto);
        Task<EmployeDto?> UpdateAsync(Guid id, UpdateEmployeDto dto);
        Task<bool> DeleteAsync(Guid id);
        // Additional methods for entity access (used by services)
        Task<Employe?> GetEntityByIdAsync(Guid id);
        Task UpdateEntityAsync(Employe employe);
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(Employe employe);
        Task SaveChangesAsync();
    }
}
