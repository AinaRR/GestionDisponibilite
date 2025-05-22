namespace GestionDisponibilite.Model
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<EmployeRole> EmployeRoles { get; set; } = new List<EmployeRole>();
    }
}
