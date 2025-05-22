using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class UpdateEmployeDto
    {
        [Required(ErrorMessage = "Le nom est requis.")]
        public string Nom { get; set; } = default!;

        [Required(ErrorMessage = "Le prénom est requis.")]
        public string Prenom { get; set; } = default!;

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; } = default!;

        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string? Telephone { get; set; }

        public string? Adresse { get; set; }

        public string? Username { get; set; }

        public string? Degree { get; set; }
    }
}
