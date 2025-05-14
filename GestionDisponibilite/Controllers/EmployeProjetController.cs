using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDisponibilite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeProjetController : ControllerBase
    {
        private readonly IEmployeProjetRepository _employeProjetRepository;

        public EmployeProjetController(IEmployeProjetRepository repository)
        {
            _employeProjetRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeProjetDetailDto>>> GetAll()
        {
            var list = await _employeProjetRepository.GetAllAsync();
            return Ok(list);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeProjetDetailDto?>> GetById(Guid id)
        {
            var item = await _employeProjetRepository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeProjetDto>> Create([FromBody] CreateEmployeProjetDto dto)
        {
            var created = await _employeProjetRepository.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id}, created);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _employeProjetRepository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
