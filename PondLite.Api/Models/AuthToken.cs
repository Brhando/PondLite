using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PondLite.Api.Models
{
    public class AuthToken
    {
        [Key]
        public Guid TokenId { get; set; } = Guid.NewGuid();

        public Guid UserAccountId { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public UserAccount? UserAccount { get; set; }

        public string TokenValue { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(30);

        public bool IsActive { get; set; } = true;
    }
}