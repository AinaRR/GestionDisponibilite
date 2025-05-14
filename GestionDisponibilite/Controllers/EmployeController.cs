using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using GestionDisponibilite.Role;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GestionDisponibilite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        private readonly IEmployeRepository _employeRepository;
        private readonly ILogger<EmployeController> _logger;

        public EmployeController(IEmployeRepository employeRepository, ILogger<EmployeController> logger)
        {
            _employeRepository = employeRepository ?? throw new ArgumentNullException(nameof(employeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// Retrieve all employees.
        [HttpGet]
        [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
        [ProducesResponseType(typeof(IEnumerable<EmployeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employes = await _employeRepository.GetAllAsync();
            return Ok(employes);
        }

        /// Retrieve a specific employee by ID.
        [HttpGet("{id:guid}", Name = nameof(GetById))]
        [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employe = await _employeRepository.GetByIdAsync(id);
            return employe is null ? NotFound($"Employé avec ID '{id}' introuvable.") : Ok(employe);
        }

        /// Create a new employee. Only accessible by Admins.
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeDto dto)
        {
            if (dto == null) return BadRequest("Données de création de l'employé manquantes.");

            try
            {
                var createdEmploye = await _employeRepository.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdEmploye.Id }, createdEmploye);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erreur lors de la création de l'employé.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la création de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");
            }
        }

        /// Update an existing employee. Only accessible by Admins.
        [HttpPut("{id:guid}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeDto dto)
        {
            if (dto == null) return BadRequest("Données de mise à jour manquantes.");

            try
            {
                var updatedEmploye = await _employeRepository.UpdateAsync(id, dto);
                return updatedEmploye is null
                    ? NotFound($"Aucun employé trouvé avec l'ID '{id}'.")
                    : Ok(updatedEmploye);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erreur lors de la mise à jour de l'employé.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la mise à jour de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne s'est produite.");
            }
        }

        /// Delete an employee. Only accessible by Admins.
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _employeRepository.DeleteAsync(id);
            return deleted
                ? NoContent()
                : NotFound($"Employé avec ID '{id}' non trouvé.");
        }
    }
}
