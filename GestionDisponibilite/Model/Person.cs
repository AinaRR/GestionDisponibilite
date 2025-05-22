namespace GestionDisponibilite.Model
{
    public class Person
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DateDeNaissance { get; set; }
        public string Degree { get; set; } = string.Empty;
    }
}
