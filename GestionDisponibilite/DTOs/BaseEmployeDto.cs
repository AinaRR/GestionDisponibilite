using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.DTOs
{
    public class BaseEmployeDto
    {
        public string Nom { get; set; } = default!;
        public string Prenom { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
