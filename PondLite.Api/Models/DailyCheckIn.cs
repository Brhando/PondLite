using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PondLite.Api.Models.Enums;

namespace PondLite.Api.Models
{
    public class DailyCheckIn
    {
        [Key]
        public Guid DailyCheckInId { get; set; } = Guid.NewGuid();

        public Guid RelationshipId { get; set; }

        [ForeignKey(nameof(RelationshipId))]
        public Relationship? Relationship { get; set; }

        public Guid UserAccountId { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public UserAccount? UserAccount { get; set; }

        public DateOnly CheckInDate { get; set; }

        public Emotion PrimaryEmotion { get; set; }

        public Emotion? SecondaryEmotion { get; set; }

        public Emotion? TertiaryEmotion { get; set; }

        public string? OptionalMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}