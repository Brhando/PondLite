using PondLite.Api.DTOs.Relationship;

namespace PondLite.Api.Services
{
    public interface IRelationshipService
    {
        Task<RelationshipResponse?> CreateRelationshipAsync(
            Guid userAccountId,
            CreateRelationshipRequest request);

        Task<RelationshipResponse?> GetActiveRelationshipForUserAsync(
            Guid userAccountId);

        Task<bool> UserBelongsToRelationshipAsync(
            Guid userAccountId,
            Guid relationshipId);

        Task<RelationshipResponse?> JoinRelationshipAsync(
            Guid userAccountId,
            JoinRelationshipRequest request);
    }
}