namespace GestionDisponibilite.Model
{
    public class EmployeRole
    {
        public Guid EmployeId { get; set; }
        public Employe Employe { get; set; } = null!;

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
