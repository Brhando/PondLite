using Microsoft.EntityFrameworkCore;
using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfRelationshipMemberRepository : IRelationshipMemberRepository
    {
        private readonly PondLiteDbContext _context;

        public EfRelationshipMemberRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(RelationshipMember relationshipMember)
        {
            _context.RelationshipMembers.Add(relationshipMember);
            _context.SaveChanges();
        }

        public RelationshipMember? GetByUserAccountId(Guid userAccountId)
        {
            return _context.RelationshipMembers
                .FirstOrDefault(member => member.UserAccountId == userAccountId);
        }

        public RelationshipMember? GetByUserAccountIdWithRelationship(Guid userAccountId)
        {
            return _context.RelationshipMembers
                .Include(member => member.Relationship)
                .FirstOrDefault(member => member.UserAccountId == userAccountId);
        }

        public bool UserBelongsToRelationship(Guid userAccountId, Guid relationshipId)
        {
            return _context.RelationshipMembers.Any(member =>
                member.UserAccountId == userAccountId &&
                member.RelationshipId == relationshipId);
        }

        public bool RelationshipHasMember(Guid relationshipId, Guid userAccountId)
        {
            return _context.RelationshipMembers.Any(member =>
                member.RelationshipId == relationshipId &&
                member.UserAccountId == userAccountId);
        }

        public List<RelationshipMember> GetByRelationshipId(Guid relationshipId)
        {
            return _context.RelationshipMembers
                .Where(member => member.RelationshipId == relationshipId)
                .ToList();
        }
    }
}