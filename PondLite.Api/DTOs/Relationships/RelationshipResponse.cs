namespace PondLite.Api.DTOs.Relationship
{
    public class RelationshipResponse
    {
        public Guid RelationshipId { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid RelationshipMemberId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTime JoinedAt { get; set; }
    }
}