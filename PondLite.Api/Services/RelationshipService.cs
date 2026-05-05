using Microsoft.EntityFrameworkCore;
using PondLite.Api.Data;
using PondLite.Api.DTOs.Relationship;
using PondLite.Api.Models;

namespace PondLite.Api.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly PondLiteDbContext _context;

        public RelationshipService(PondLiteDbContext context)
        {
            _context = context;
        }

        public async Task<RelationshipResponse?> CreateRelationshipAsync(
            Guid userAccountId,
            CreateRelationshipRequest request)
        {
            var existingMembership = await _context.RelationshipMembers
                .FirstOrDefaultAsync(member => member.UserAccountId == userAccountId);

            if (existingMembership != null)
            {
                return null;
            }

            var relationship = new Relationship
            {
                Name = string.IsNullOrWhiteSpace(request.Name)
                    ? "Our Pond"
                    : request.Name.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var relationshipMember = new RelationshipMember
            {
                RelationshipId = relationship.RelationshipId,
                UserAccountId = userAccountId,
                DisplayName = string.IsNullOrWhiteSpace(request.DisplayName)
                    ? "PondMate"
                    : request.DisplayName.Trim(),
                Role = "Owner",
                JoinedAt = DateTime.UtcNow
            };

            _context.Relationships.Add(relationship);
            _context.RelationshipMembers.Add(relationshipMember);

            await _context.SaveChangesAsync();

            return new RelationshipResponse
            {
                RelationshipId = relationship.RelationshipId,
                Name = relationship.Name,
                RelationshipMemberId = relationshipMember.RelationshipMemberId,
                DisplayName = relationshipMember.DisplayName,
                Role = relationshipMember.Role,
                JoinedAt = relationshipMember.JoinedAt
            };
        }

        public async Task<RelationshipResponse?> GetActiveRelationshipForUserAsync(
            Guid userAccountId)
        {
            var membership = await _context.RelationshipMembers
                .Include(member => member.Relationship)
                .FirstOrDefaultAsync(member => member.UserAccountId == userAccountId);

            if (membership == null || membership.Relationship == null)
            {
                return null;
            }

            return new RelationshipResponse
            {
                RelationshipId = membership.Relationship.RelationshipId,
                Name = membership.Relationship.Name,
                RelationshipMemberId = membership.RelationshipMemberId,
                DisplayName = membership.DisplayName,
                Role = membership.Role,
                JoinedAt = membership.JoinedAt
            };
        }

        public async Task<bool> UserBelongsToRelationshipAsync(
            Guid userAccountId,
            Guid relationshipId)
        {
            return await _context.RelationshipMembers
                .AnyAsync(member =>
                    member.UserAccountId == userAccountId &&
                    member.RelationshipId == relationshipId);
        }
    }
}