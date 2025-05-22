using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class CreateEmployeDto : BaseEmployeDto
    {
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MinLength(8)]
        public string Password { get; set; } = default!;

        public string? Degree { get; set; }

        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        public string Username { get; set; } = default!;   // ← Obligatoire

        public string Role { get; set; } = "User";
    }
}
