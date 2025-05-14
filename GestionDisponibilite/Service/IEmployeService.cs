using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;

namespace GestionDisponibilite.Service
{
    public interface IEmployeService
    {
        Task<EmployeProjetDto> LinkEmployeToProjetAsync(Guid employeId, Guid projetId);
        Task<EmployeDto> CreateEmployeAsync(CreateEmployeDto dto);
        Task<EmployeDto> RegisterEmployeAsync(RegisterEmployeDto dto);
    }
}
