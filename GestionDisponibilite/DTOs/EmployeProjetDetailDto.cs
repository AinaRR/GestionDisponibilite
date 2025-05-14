namespace GestionDisponibilite.DTOs
{
    public class EmployeProjetDetailDto
    {
        public Guid Id { get; set; } 
        public string EmployeNom { get; set; } = string.Empty;
        public string ProjetNom { get; set; } = string.Empty;
        public DateOnly DateFinProjet { get; set; }
    }
}
