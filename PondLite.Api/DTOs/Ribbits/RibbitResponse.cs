using PondLite.Api.Models.Enums;

namespace PondLite.Api.DTOs.Ribbits
{
    public class RibbitResponse
    {
        public Guid RibbitId { get; set; }

        public Guid RelationshipId { get; set; }

        public Guid SenderUserId { get; set; }

        public Guid ReceiverUserId { get; set; }

        public RibbitType Type { get; set; }

        public RibbitStatus Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? AcknowledgementEmoji { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? AcknowledgedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime? CancelledAt { get; set; }
    }
}