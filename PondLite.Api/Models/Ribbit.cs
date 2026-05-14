using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PondLite.Api.Models.Enums;

namespace PondLite.Api.Models
{
    public class Ribbit
    {
        [Key]
        public Guid RibbitId { get; set; } = Guid.NewGuid();

        public Guid RelationshipId { get; set; }

        [ForeignKey(nameof(RelationshipId))]
        public Relationship? Relationship { get; set; }

        public Guid SenderUserId { get; set; }

        [ForeignKey(nameof(SenderUserId))]
        public UserAccount? SenderUser { get; set; }

        public Guid ReceiverUserId { get; set; }

        [ForeignKey(nameof(ReceiverUserId))]
        public UserAccount? ReceiverUser { get; set; }

        public RibbitType Type { get; set; } = RibbitType.Thought;

        public RibbitStatus Status { get; set; } = RibbitStatus.Sent;

        public string Message { get; set; } = string.Empty;

        public string? AcknowledgementEmoji { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? AcknowledgedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime? CancelledAt { get; set; }
    }
}