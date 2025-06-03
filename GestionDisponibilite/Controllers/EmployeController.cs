using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using GestionDisponibilite.RoleUser;
using GestionDisponibilite.Service;
using GestionDisponibilite.Support;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GestionDisponibilite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class EmployeController : ControllerBase
    {
        private readonly IEmployeService _employeService;
        private readonly ILogger<EmployeController> _logger;

        public EmployeController(IEmployeService employeService, ILogger<EmployeController> logger)
        {
            _employeService = employeService ?? throw new ArgumentNullException(nameof(employeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Authorize(Roles = Roles.All)]
        [ProducesResponseType(typeof(IEnumerable<EmployeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeDto>>> GetAll()
        {
            var employes = await _employeService.GetAllAsync();
            return Ok(employes);
        }

        [HttpGet("{id:guid}", Name = nameof(GetById))]
        [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeDto>> GetById(Guid id)
        {
            var employe = await _employeService.GetByIdAsync(id);
            if (employe is null)
            {
                _logger.LogInformation("Aucun employé trouvé avec l'ID : {Id}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Employé introuvable",
                    Detail = ErrorMessages.EmployeNotFound(id),
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(employe);
        }

        [HttpGet("{id:guid}/projets")]
        [Authorize(Roles = Roles.All)]
        [ProducesResponseType(typeof(IEnumerable<ProjetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProjetDto>>> GetProjetsByEmployeId(Guid id)
        {
            try
            {
                var projets = await _employeService.GetProjetsByEmployeIdAsync(id);
                return Ok(projets);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erreur lors de la récupération des projets pour l'employé {Id}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Employé introuvable",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        [HttpPost("{employeId:guid}/projets/{projetId:guid}")]
        [Authorize(Roles = Roles.AdminAndSuperAdmin)]
        [ProducesResponseType(typeof(EmployeProjetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeProjetDto>> LinkEmployeToProjet(Guid employeId, Guid projetId)
        {
            try
            {
                var linked = await _employeService.LinkEmployeToProjetAsync(employeId, projetId);
                return Created(string.Empty, linked);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erreur métier lors de l'association employé-projet : {Message}", ex.Message);
                return BadRequest(new ProblemDetails
                {
                    Title = "Erreur de validation métier",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de l'association employé-projet.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Erreur serveur",
                    Detail = ErrorMessages.InternalServerError,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminAndSuperAdmin)]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeDto>> Create([FromBody] CreateEmployeDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Données de création nulles.");
                return BadRequest("Les données de création sont requises.");
            }

            try
            {
                var createdEmploye = await _employeService.CreateEmployeAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdEmploye.Id }, createdEmploye);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erreur métier lors de la création : {Message}", ex.Message);
                return BadRequest(new ProblemDetails
                {
                    Title = "Erreur de validation métier",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la création de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Erreur serveur",
                    Detail = ErrorMessages.InternalServerError,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = Roles.AdminAndSuperAdmin)]
        [ProducesResponseType(typeof(EmployeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeDto>> Update(Guid id, [FromBody] UpdateEmployeDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Données de mise à jour nulles pour l'ID : {Id}", id);
                return BadRequest("Les données de mise à jour sont requises.");
            }

            try
            {
                var updatedEmploye = await _employeService.UpdateEmployeAsync(id, dto);
                if (updatedEmploye is null)
                {
                    _logger.LogInformation("Aucun employé trouvé pour mise à jour avec l'ID : {Id}", id);
                    return NotFound(new ProblemDetails
                    {
                        Title = "Employé introuvable",
                        Detail = ErrorMessages.EmployeNotFound(id),
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Ok(updatedEmploye);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erreur métier lors de la mise à jour : {Message}", ex.Message);
                return BadRequest(new ProblemDetails
                {
                    Title = "Erreur de validation métier",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la mise à jour de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Erreur serveur",
                    Detail = ErrorMessages.InternalServerError,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = Roles.AdminAndSuperAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _employeService.DeleteEmployeAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Suppression échouée : employé introuvable avec l'ID : {Id}", id);
                    return NotFound(new ProblemDetails
                    {
                        Title = "Employé introuvable",
                        Detail = ErrorMessages.EmployeNotFound(id),
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la suppression de l'employé.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Erreur serveur",
                    Detail = ErrorMessages.InternalServerError,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}