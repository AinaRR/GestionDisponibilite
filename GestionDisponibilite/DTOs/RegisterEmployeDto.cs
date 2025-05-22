using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class RegisterEmployeDto : BaseEmployeDto
    {
        public string Password { get; set; } = default!;
        public string? Telephone { get; set; }
        public DateOnly? DateDeNaissance { get; set; }
        public string? Adresse { get; set; }
        public string? Degree { get; set; }
    }
}
