namespace GestionDisponibilite.DTOs
{
    public class TokenResponseDto
    {
        public string Token { get; set; } = null!;
        public int ExpirationMinutes { get; set; }
        public string Role { get; set; } = null!;
    }
}
