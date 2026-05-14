namespace PondLite.Api.DTOs.Relationship
{
    public class JoinRelationshipRequest
    {
        public Guid RelationshipId { get; set; }

        public string DisplayName { get; set; } = string.Empty;
    }
}