using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PondLite.Api.Models
{
    public class RelationshipMember
    {
        [Key]
        public Guid RelationshipMemberId { get; set; } = Guid.NewGuid();

        public Guid RelationshipId { get; set; }

        [ForeignKey(nameof(RelationshipId))]
        public Relationship? Relationship { get; set; }

        public Guid UserAccountId { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public UserAccount? UserAccount { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string Role { get; set; } = "Member";

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public Frog? Frog { get; set; }
    }
}
