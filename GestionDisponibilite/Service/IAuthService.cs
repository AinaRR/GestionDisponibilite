using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Service
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> AuthenticateAsync(string login, string password);
    }
}
