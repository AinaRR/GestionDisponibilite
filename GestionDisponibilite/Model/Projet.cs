using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDisponibilite.Model
{
    public class Projet
    {
        [Key]
        public Guid ProjetId { get; set; } = Guid.NewGuid(); // Set in C# not SQL
        public string NomDuProjet { get; set; } = string.Empty;
        public string? Client { get; set; }
        public DateOnly DebutProjet { get; set; }
        public DateOnly FinProjet { get; set; }
        public string Status { get; set; } = "En cours";
        public int NombreDeEmploye => EmployeProjets?.Count ?? 0;
        public ICollection<EmployeProjet> EmployeProjets { get; set; } = new List<EmployeProjet>();
    }
}
