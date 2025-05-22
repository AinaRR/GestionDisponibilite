using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Service;
using GestionDisponibilite.Model;
using GestionDisponibilite.RoleUser;
using GestionDisponibilite.Extensions;


namespace GestionDisponibilite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmployeService _employeService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IEmployeService employeService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _employeService = employeService;
            _logger = logger;
        }

        // ─────────────────────────────────────────────
        // LOGIN (email OU username)
        // ─────────────────────────────────────────────
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.AuthenticateAsync(dto.Username, dto.Password);

            if (response == null)
            {
                _logger.LogWarning("Échec de connexion pour le user {Username}", dto.Username);
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");
            }

            return Ok(response);   // Token, ExpirationMinutes, Role
        }

        // ─────────────────────────────────────────────
        // REGISTER (public)
        // ─────────────────────────────────────────────
        [HttpPost("public/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeDto dto)
        {
            try
            {
                var employe = await _employeService.RegisterAsync(dto, isAdmin: false);

                return CreatedAtAction(nameof(EmployeController.GetById),
                                       "Employe",
                                       new { id = employe.Id },
                                       employe);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Échec de l'enregistrement anonyme.");
                return BadRequest(new { message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────
        // REGISTER par ADMIN
        // ─────────────────────────────────────────────
        [HttpPost("admin/register")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterEmployeDto dto)
        {
            try
            {
                var employe = await _employeService.RegisterAsync(dto, isAdmin: true);

                return CreatedAtAction(nameof(EmployeController.GetById),
                                       "Employe",
                                       new { id = employe.Id },
                                       employe);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Échec de l'enregistrement par un administrateur.");
                return BadRequest(new { message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────
        // TEST HASH (public)
        // ─────────────────────────────────────────────
        [HttpGet("test-password-hash")]
        [AllowAnonymous]
        public IActionResult TestPasswordHash([FromServices] IPasswordHasher hasher)
        {
            const string pwd = "monMotDePasseSécurisé123!";
            var hash = hasher.HashPassword(pwd);

            return Ok(new
            {
                Password = pwd,
                Hash = hash,
                IsValid = hasher.VerifyPassword(hash, pwd)
            });
        }

        // ─────────────────────────────────────────────
        // /api/auth/me – profil utilisateur courant
        // ─────────────────────────────────────────────
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.GetUserId();

            return Ok(new
            {
                UserId = userId,
                Username = User.Identity?.Name,
                Role = User.FindFirstValue(ClaimTypes.Role)
            });
        }
    }
}
