namespace GestionDisponibilite.Model
{
    public class Person
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public DateOnly DateDeNaissance { get; set; }
        public string Degree { get; set; }
    }
}
