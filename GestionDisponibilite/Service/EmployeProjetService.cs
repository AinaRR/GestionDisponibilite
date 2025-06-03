using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using Mapster;

namespace GestionDisponibilite.Service
{   

    public class EmployeProjetService : IEmployeProjetService
    {
        private readonly IEmployeProjetRepository _repository;
        private readonly IEmployeRepository _employeRepository;
        private readonly IProjetRepository _projetRepository;
        private readonly ILogger<EmployeProjetService> _logger;

        public EmployeProjetService(
            IEmployeProjetRepository repository,
            IEmployeRepository employeRepository,
            IProjetRepository projetRepository,
            ILogger<EmployeProjetService> logger)
        {
            _repository = repository;
            _employeRepository = employeRepository;
            _projetRepository = projetRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeProjetDetailDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<EmployeProjetDetailDto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<EmployeProjetDto> CreateAsync(CreateEmployeProjetDto dto)
        {
            var employeExists = await _employeRepository.ExistsByIdAsync(dto.EmployeId);
            var projetExists = await _projetRepository.ExistsByIdAsync(dto.ProjetId);

            if (!employeExists)
                throw new DomainException($"Aucun employé trouvé avec l'ID {dto.EmployeId}.");
            if (!projetExists)
                throw new DomainException($"Aucun projet trouvé avec l'ID {dto.ProjetId}.");

            if (await _repository.GetByEmployeAndProjetAsync(dto.EmployeId, dto.ProjetId) is not null)
                throw new DomainException("L'employé est déjà associé à ce projet.");

            var entity = new EmployeProjet
            {
                Id = Guid.NewGuid(),
                EmployeId = dto.EmployeId,
                ProjetId = dto.ProjetId
            };

            await using var tx = await _repository.BeginTransactionAsync();
            try
            {
                var created = await _repository.CreateRawAsync(entity);
                await tx.CommitAsync();
                return created.Adapt<EmployeProjetDto>();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, "Erreur lors de la création de l'association employé-projet.");
                throw new DomainException("Erreur lors de l'association employé-projet.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
