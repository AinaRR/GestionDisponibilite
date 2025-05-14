using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class CreateEmployeDto : BaseEmployeDto
    {
        public string? Degree { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; } = "User";
    }
}
