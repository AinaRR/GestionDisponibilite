using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using System.Security.Claims;

namespace GestionDisponibilite.Service
{
    public interface IEmployeService
    {
        Task<EmployeDto> CreateEmployeAsync(CreateEmployeDto dto);
        Task<EmployeDto> RegisterAsync(RegisterEmployeDto dto, bool isAdmin = false); // version Admin
        Task<EmployeProjetDto> LinkEmployeToProjetAsync(Guid employeId, Guid projetId);
    }
}
