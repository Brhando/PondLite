using PondLite.Api.DTOs.Frogs;
using PondLite.Api.Models;
using PondLite.Api.Repositories;

namespace PondLite.Api.Services
{
    public class FrogService : IFrogService
    {
        private readonly IFrogRepository _frogRepository;
        private readonly IRelationshipMemberRepository _relationshipMemberRepository;

        public FrogService(
            IFrogRepository frogRepository,
            IRelationshipMemberRepository relationshipMemberRepository)
        {
            _frogRepository = frogRepository;
            _relationshipMemberRepository = relationshipMemberRepository;
        }

        public Task<FrogResponse?> GetMyFrogAsync(Guid userAccountId)
        {
            RelationshipMember? relationshipMember =
                _relationshipMemberRepository.GetByUserAccountId(userAccountId);

            if (relationshipMember == null)
            {
                return Task.FromResult<FrogResponse?>(null);
            }

            Frog? frog =
                _frogRepository.GetByRelationshipMemberId(
                    relationshipMember.RelationshipMemberId);

            if (frog == null)
            {
                return Task.FromResult<FrogResponse?>(null);
            }

            FrogResponse response = BuildFrogResponse(frog);

            return Task.FromResult<FrogResponse?>(response);
        }

        public Task<FrogResponse?> UpdateMyFrogAsync(
            Guid userAccountId,
            UpdateFrogRequest request)
        {
            RelationshipMember? relationshipMember =
                _relationshipMemberRepository.GetByUserAccountId(userAccountId);

            if (relationshipMember == null)
            {
                return Task.FromResult<FrogResponse?>(null);
            }

            Frog? frog =
                _frogRepository.GetByRelationshipMemberId(
                    relationshipMember.RelationshipMemberId);

            if (frog == null)
            {
                return Task.FromResult<FrogResponse?>(null);
            }

            frog.Name = request.Name.Trim();
            frog.LastUpdatedAt = DateTime.UtcNow;

            _frogRepository.Update(frog);

            FrogResponse response = BuildFrogResponse(frog);

            return Task.FromResult<FrogResponse?>(response);
        }

        private static FrogResponse BuildFrogResponse(Frog frog)
        {
            return new FrogResponse
            {
                FrogId = frog.FrogId,
                RelationshipMemberId = frog.RelationshipMemberId,
                Name = frog.Name,
                CurrentMood = frog.CurrentMood.ToString(),
                ActivityState = frog.ActivityState.ToString(),
                LastUpdatedAt = frog.LastUpdatedAt
            };
        }
    }
}
