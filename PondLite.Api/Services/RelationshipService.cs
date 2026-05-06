using PondLite.Api.DTOs.Relationship;
using PondLite.Api.Models;
using PondLite.Api.Repositories;

namespace PondLite.Api.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IRelationshipMemberRepository _relationshipMemberRepository;
        private readonly IFrogService _frogService;

        public RelationshipService(
            IRelationshipRepository relationshipRepository,
            IRelationshipMemberRepository relationshipMemberRepository,
            IFrogService frogService)
        {
            _relationshipRepository = relationshipRepository;
            _relationshipMemberRepository = relationshipMemberRepository;
            _frogService = frogService;
        }

        public async Task<RelationshipResponse?> CreateRelationshipAsync(
            Guid userAccountId,
            CreateRelationshipRequest request)
        {
            RelationshipMember? existingMembership =
                _relationshipMemberRepository.GetByUserAccountId(userAccountId);

            if (existingMembership != null)
            {
                return null;
            }

            Relationship relationship = new Relationship
            {
                Name = string.IsNullOrWhiteSpace(request.Name)
                    ? "Our Pond"
                    : request.Name.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            RelationshipMember relationshipMember = new RelationshipMember
            {
                RelationshipId = relationship.RelationshipId,
                UserAccountId = userAccountId,
                DisplayName = string.IsNullOrWhiteSpace(request.DisplayName)
                    ? "PondMate"
                    : request.DisplayName.Trim(),
                Role = "Owner",
                JoinedAt = DateTime.UtcNow
            };

            _relationshipRepository.Add(relationship);
            _relationshipMemberRepository.Add(relationshipMember);

            await _frogService.CreateDefaultFrogForRelationshipMemberAsync(
                relationshipMember.RelationshipMemberId);

            RelationshipResponse response = new RelationshipResponse
            {
                RelationshipId = relationship.RelationshipId,
                Name = relationship.Name,
                RelationshipMemberId = relationshipMember.RelationshipMemberId,
                DisplayName = relationshipMember.DisplayName,
                Role = relationshipMember.Role,
                JoinedAt = relationshipMember.JoinedAt
            };

            return response;
        }

        public Task<RelationshipResponse?> GetActiveRelationshipForUserAsync(
            Guid userAccountId)
        {
            RelationshipMember? membership =
                _relationshipMemberRepository.GetByUserAccountIdWithRelationship(userAccountId);

            if (membership == null || membership.Relationship == null)
            {
                return Task.FromResult<RelationshipResponse?>(null);
            }

            RelationshipResponse response = new RelationshipResponse
            {
                RelationshipId = membership.Relationship.RelationshipId,
                Name = membership.Relationship.Name,
                RelationshipMemberId = membership.RelationshipMemberId,
                DisplayName = membership.DisplayName,
                Role = membership.Role,
                JoinedAt = membership.JoinedAt
            };

            return Task.FromResult<RelationshipResponse?>(response);
        }

        public Task<bool> UserBelongsToRelationshipAsync(
            Guid userAccountId,
            Guid relationshipId)
        {
            bool belongsToRelationship =
                _relationshipMemberRepository.UserBelongsToRelationship(
                    userAccountId,
                    relationshipId);

            return Task.FromResult(belongsToRelationship);
        }
    }
}
