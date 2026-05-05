using System.ComponentModel.DataAnnotations;
namespace PondLite.Api.Models
{
    public class UserAccount
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
