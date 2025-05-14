using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class RegisterEmployeDto : BaseEmployeDto
    {
        public string? Telephone { get; set; }
        public DateOnly? DateDeNaissance { get; set; }
        public string? Adresse { get; set; }
    }
}
