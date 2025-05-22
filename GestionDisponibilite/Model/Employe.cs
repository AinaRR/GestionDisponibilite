using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;

namespace GestionDisponibilite.Model
{
    public class Employe : Person
    {
        [Key]
        public Guid EmployeId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "User";
        public bool SurProjet { get; set; }

        [NotMapped]
        public int NombreDeProjet => EmployeProjets?.Count ?? 0;
        public ICollection<EmployeProjet> EmployeProjets { get; set; } = new List<EmployeProjet>();
        public ICollection<EmployeRole> EmployeRoles { get; set; } = new List<EmployeRole>();
    }
}
