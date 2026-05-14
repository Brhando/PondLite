using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IRelationshipMemberRepository
    {
        void Add(RelationshipMember relationshipMember);

        RelationshipMember? GetByUserAccountId(Guid userAccountId);

        RelationshipMember? GetByUserAccountIdWithRelationship(Guid userAccountId);

        bool UserBelongsToRelationship(Guid userAccountId, Guid relationshipId);

        bool RelationshipHasMember(Guid relationshipId, Guid userAccountId);

        int CountMembersForRelationship(Guid relationshipId);

        List<RelationshipMember> GetByRelationshipId(Guid relationshipId);
    }
}