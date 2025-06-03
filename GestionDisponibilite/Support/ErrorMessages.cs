namespace GestionDisponibilite.Support
{
    public static class ErrorMessages
    {
        public static string EmployeNotFound(Guid id) => $"Aucun employé trouvé avec l'ID '{id}'.";
        public const string InvalidCreateModel = "Modèle de création invalide.";
        public const string InvalidUpdateModel = "Modèle de mise à jour invalide.";
        public const string InternalServerError = "Erreur interne du serveur.";
    }
}
