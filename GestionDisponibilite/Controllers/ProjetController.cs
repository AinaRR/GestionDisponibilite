using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDisponibilite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetController : ControllerBase
    {
        private readonly IProjetRepository _projetRepository;

        public ProjetController(IProjetRepository projetRepository)
        {
            _projetRepository = projetRepository;
        }

        // GET: api/projet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetDto>>> GetAll()
        {
            var projets = await _projetRepository.GetAll();
            return projets.Any() ? Ok(projets) : NoContent();
        }

        // GET: api/projet/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProjetDto>> GetById(Guid id)
        {
            var projet = await _projetRepository.GetById(id);
            return projet is null
                ? NotFound($"Projet with ID {id} not found.")
                : Ok(projet);
        }

        // POST: api/projet
        [HttpPost]
        public async Task<ActionResult<ProjetDto>> Create([FromBody] CreateProjetDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdProjet = await _projetRepository.Add(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdProjet.ProjetId }, createdProjet);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/projet/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProjetDto>> Update(Guid id, [FromBody] UpdateProjetDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedProjet = await _projetRepository.Update(id, dto);
                return updatedProjet is null
                    ? NotFound($"Projet with ID {id} not found.")
                    : Ok(updatedProjet);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/projet/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _projetRepository.Delete(id);
            return deleted ? NoContent() : NotFound($"Projet with ID {id} not found.");
        }
    }
}
