using System.ComponentModel.DataAnnotations;
using PondLite.Api.Models.Enums;

namespace PondLite.Api.Models
{
    public class UserAccount
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public AccountType AccountType { get; set; } = AccountType.User;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RelationshipMember> RelationshipMembers { get; set; }
            = new List<RelationshipMember>();
    }
}