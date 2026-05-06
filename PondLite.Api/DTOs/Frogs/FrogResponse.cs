namespace PondLite.Api.DTOs.Frogs
{
    public class FrogResponse
    {
        public Guid FrogId { get; set; }

        public Guid RelationshipMemberId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string CurrentMood { get; set; } = string.Empty;

        public string ActivityState { get; set; } = string.Empty;

        public DateTime LastUpdatedAt { get; set; }
    }
}
