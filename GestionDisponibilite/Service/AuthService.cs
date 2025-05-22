using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using GestionDisponibilite.Repository;
using GestionDisponibilite.RoleUser;
using System.Security.Cryptography;
using System.Text;

namespace GestionDisponibilite.Service
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeRepository _employeRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;

        // Injection du PasswordHasher via constructeur
        public AuthService(
            IEmployeRepository employeRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHasher passwordHasher)
        {
            _employeRepository = employeRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenResponseDto?> AuthenticateAsync(string login, string password)
        {
            // Récupération de l'employé selon login (email ou username)
            Employe? employe = login.Contains('@')
                ? await _employeRepository.GetByEmailAsync(login)
                : await _employeRepository.GetByUsernameAsync(login);

            // Vérifie que l'employé existe et que le mot de passe est correct
            if (employe == null)
                return null;

            bool passwordValid = _passwordHasher.VerifyPassword(employe.PasswordHash, password);
            if (!passwordValid)
                return null;

            // Récupère le rôle ou utilise un rôle par défaut
            var role = string.IsNullOrEmpty(employe.Role) ? Roles.User : employe.Role;

            // Génère le token JWT
            var token = _jwtTokenService.GenerateToken(
                userId: employe.EmployeId.ToString(),
                username: employe.Username,
                role: role
            );

            // Retourne la réponse avec token + durée + rôle
            return new TokenResponseDto
            {
                Token = token,
                ExpirationMinutes = 60,   // ou la durée réelle de validité de ton token
                Role = role
            };
        }
    }
}

