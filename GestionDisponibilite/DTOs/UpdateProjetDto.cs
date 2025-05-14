namespace GestionDisponibilite.DTOs
{
    public class UpdateProjetDto
    {
        public string NomDuProjet { get; set; } = default!;
        public string? Client { get; set; }
        public DateOnly DebutProjet { get; set; }
        public DateOnly FinProjet { get; set; }
        public string Status { get; set; } = default!;
    }
}
