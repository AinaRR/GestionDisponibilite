using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class CreateProjetDto
    {
        public string NomDuProjet { get; set; } = default!;
        public string? Client { get; set; }
        public DateOnly DebutProjet { get; set; }
        public DateOnly FinProjet { get; set; }

        [Required]
        [RegularExpression("^(En cours|Terminé)$", ErrorMessage = "Invalid status. Allowed values are 'En cours' and 'Terminé'.")]
        public string Status { get; set; } = default!;
    }
}
