namespace GestionDisponibilite.DTOs
{
    public class EmployeDto
    {
        public Guid Id { get; set; }
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public required string Email { get; set; }
        public int NombreDeProjet { get; set; }
    }
}
