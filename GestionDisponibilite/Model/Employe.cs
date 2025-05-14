using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;

namespace GestionDisponibilite.Model
{
    public class Employe : Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EmployeID { get; set; } = Guid.NewGuid(); // Generate in code
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool SurProjet { get; set; }
        [NotMapped]
        public int NombreDeProjet => EmployeProjets?.Count ?? 0;
        public ICollection<EmployeProjet> EmployeProjets { get; set; } = new List<EmployeProjet>();
        public void SetPassword(string password) => PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        public bool VerifyPassword(string password) => BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}
