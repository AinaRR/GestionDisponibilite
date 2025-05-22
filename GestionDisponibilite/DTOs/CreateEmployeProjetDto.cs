using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class CreateEmployeProjetDto
    {
        public Guid EmployeId { get; set; }
        public Guid ProjetId { get; set; }
    }
}
