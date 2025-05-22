namespace GestionDisponibilite.Service
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string username, string role);
    }
}
