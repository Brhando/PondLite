namespace PondLite.Api.DTOs.CheckIns
{
    public class DailyCheckInResponse
    {
        public Guid DailyCheckInId { get; set; }

        public Guid RelationshipId { get; set; }

        public Guid UserAccountId { get; set; }

        public DateOnly CheckInDate { get; set; }

        public string PrimaryEmotion { get; set; } = string.Empty;

        public string? SecondaryEmotion { get; set; }

        public string? TertiaryEmotion { get; set; }

        public string? OptionalMessage { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}