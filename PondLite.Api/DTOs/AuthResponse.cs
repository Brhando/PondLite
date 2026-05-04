namespace PondLite.Api.DTOs
{
    public class AuthResponse
    {
        public Guid UserAccountId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}