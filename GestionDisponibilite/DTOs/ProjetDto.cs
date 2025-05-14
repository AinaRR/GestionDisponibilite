namespace GestionDisponibilite.DTOs
{
    public class ProjetDto
    {
        public Guid ProjetId { get; set; }
        public string NomDuProjet { get; set; }
        public string? Client { get; set; }
        public DateOnly FinProjet { get; set; }
        public int NombreDeEmploye { get; set; }
    }
}
