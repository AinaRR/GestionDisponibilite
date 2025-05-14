using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class CreateEmployeProjetDto
    {
        [Required]
        public Guid EmployeId { get; set; }
        [Required]
        public Guid ProjetId { get; set; }
    }
}
