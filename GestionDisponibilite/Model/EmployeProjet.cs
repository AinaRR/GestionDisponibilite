using System.ComponentModel.DataAnnotations;

namespace GestionDisponibilite.Model
{
    public class EmployeProjet
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EmployeId { get; set; }
        public Employe Employe { get; set; }
        public Guid ProjetId { get; set; }
        public Projet Projet { get; set; }
    }
}
