using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using GestionDisponibilite.RoleUser;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GestionDisponibilite.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des employés.
    /// </summary>
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

        /// <summary>
        /// Récupère la liste de tous les employés.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
        [ProducesResponseType(typeof(IEnumerable<EmployeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employes = await _employeRepository.GetAllAsync();
            return Ok(employes);
        }

        /// <summary>
        /// Récupère un employé par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'employé</param>
        [HttpGet("{id:guid}", Name = nameof(GetById))]
        [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employe = await _employeRepository.GetByIdAsync(id);
            if (employe is null)
            {
                _logger.LogInformation("Aucun employé trouvé avec l'ID : {Id}", id);
                return NotFound($"Aucun employé trouvé avec l'ID '{id}'.");
            }

            return Ok(employe);
        }

        /// <summary>
        /// Crée un nouvel employé (réservé aux administrateurs).
        /// </summary>
        /// <param name="dto">Données du nouvel employé</param>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Données de création nulles.");
                return BadRequest("Les données de création sont requises.");
            }

            try
            {
                var createdEmploye = await _employeRepository.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdEmploye.Id }, createdEmploye);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erreur lors de la création : {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la création de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur.");
            }
        }

        /// <summary>
        /// Met à jour un employé existant (réservé aux administrateurs).
        /// </summary>
        /// <param name="id">ID de l'employé</param>
        /// <param name="dto">Données mises à jour</param>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Données de mise à jour nulles pour l'ID : {Id}", id);
                return BadRequest("Les données de mise à jour sont requises.");
            }

            try
            {
                var updatedEmploye = await _employeRepository.UpdateAsync(id, dto);
                if (updatedEmploye is null)
                {
                    _logger.LogInformation("Aucun employé trouvé pour la mise à jour avec l'ID : {Id}", id);
                    return NotFound($"Aucun employé trouvé avec l'ID '{id}'.");
                }

                return Ok(updatedEmploye);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Conflit lors de la mise à jour : {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la mise à jour de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur.");
            }
        }

        /// <summary>
        /// Supprime un employé (réservé aux administrateurs).
        /// </summary>
        /// <param name="id">ID de l'employé</param>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _employeRepository.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Suppression échouée : employé introuvable avec l'ID : {Id}", id);
                    return NotFound($"Aucun employé trouvé avec l'ID '{id}'.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la suppression de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur.");
            }
        }
    }
}
