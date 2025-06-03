namespace GestionDisponibilite.RoleUser;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string User = "User";

    public const string All = $"{SuperAdmin},{Admin},{User}";
    public const string AdminAndSuperAdmin = $"{SuperAdmin},{Admin}";
}
