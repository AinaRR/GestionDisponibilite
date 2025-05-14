using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class UpdateEmployeDto
    {
        [Required]
        public required string Nom { get; set; }
        [Required]
        public required string Prenom { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Username { get; set; }
        public string? Degree { get; set; }
    }
}
