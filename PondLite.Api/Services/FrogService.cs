using PondLite.Api.DTOs.Frogs;
using PondLite.Api.Models;
using PondLite.Api.Models.Enums;
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

        public Task<FrogResponse> CreateDefaultFrogForRelationshipMemberAsync(
            Guid relationshipMemberId)
        {
            Frog? existingFrog =
                _frogRepository.GetByRelationshipMemberId(relationshipMemberId);

            if (existingFrog != null)
            {
                return Task.FromResult(BuildFrogResponse(existingFrog));
            }

            Frog frog = CreateDefaultFrog(relationshipMemberId);

            _frogRepository.Add(frog);

            return Task.FromResult(BuildFrogResponse(frog));
        }

        public async Task<FrogResponse?> CreateMyFrogAsync(Guid userAccountId)
        {
            RelationshipMember? relationshipMember =
                _relationshipMemberRepository.GetByUserAccountId(userAccountId);

            if (relationshipMember == null)
            {
                return null;
            }

            return await CreateDefaultFrogForRelationshipMemberAsync(
                relationshipMember.RelationshipMemberId);
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
                frog = CreateDefaultFrog(relationshipMember.RelationshipMemberId);
                _frogRepository.Add(frog);
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
                frog = CreateDefaultFrog(relationshipMember.RelationshipMemberId);
                _frogRepository.Add(frog);
            }

            frog.Name = request.Name.Trim();
            frog.LastUpdatedAt = DateTime.UtcNow;

            _frogRepository.Update(frog);

            FrogResponse response = BuildFrogResponse(frog);

            return Task.FromResult<FrogResponse?>(response);
        }

        public Task<FrogResponse?> UpdateFrogAfterCheckInAsync(
    Guid relationshipMemberId,
    Emotion primaryEmotion)
        {
            Frog? frog =
                _frogRepository.GetByRelationshipMemberId(relationshipMemberId);

            if (frog == null)
            {
                frog = CreateDefaultFrog(relationshipMemberId);
                _frogRepository.Add(frog);
            }

            frog.CurrentMood = MapEmotionToFrogMood(primaryEmotion);
            frog.ActivityState = FrogActivityState.Active;
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

        private static Frog CreateDefaultFrog(Guid relationshipMemberId)
        {
            return new Frog
            {
                RelationshipMemberId = relationshipMemberId,
                Name = "Little Frog",
                CurrentMood = FrogMood.None,
                ActivityState = FrogActivityState.Asleep,
                LastUpdatedAt = DateTime.UtcNow
            };
        }

        // helper method for mapping emotions to FrogMood
        private static FrogMood MapEmotionToFrogMood(Emotion emotion)
        {
            return emotion switch
            {
                Emotion.Happy => FrogMood.Happy,
                Emotion.Content => FrogMood.Calm,
                Emotion.Loved => FrogMood.Loving,
                Emotion.Grateful => FrogMood.Loving,
                Emotion.Excited => FrogMood.Excited,
                Emotion.Hopeful => FrogMood.Calm,
                Emotion.Peaceful => FrogMood.Calm,
                Emotion.Playful => FrogMood.Happy,
                Emotion.Proud => FrogMood.Happy,

                Emotion.Sad => FrogMood.Sad,
                Emotion.Lonely => FrogMood.Sad,
                Emotion.Tired => FrogMood.Tired,
                Emotion.Drained => FrogMood.Tired,
                Emotion.Disappointed => FrogMood.Sad,
                Emotion.Hurt => FrogMood.Sad,

                Emotion.Anxious => FrogMood.Anxious,
                Emotion.Stressed => FrogMood.Anxious,
                Emotion.Worried => FrogMood.Anxious,
                Emotion.Insecure => FrogMood.Anxious,
                Emotion.Nervous => FrogMood.Anxious,

                Emotion.Frustrated => FrogMood.Frustrated,
                Emotion.Irritated => FrogMood.Frustrated,
                Emotion.Angry => FrogMood.Frustrated,
                Emotion.Resentful => FrogMood.Frustrated,

                Emotion.Overwhelmed => FrogMood.Overwhelmed,
                Emotion.Vulnerable => FrogMood.Reflective,
                Emotion.Thoughtful => FrogMood.Reflective,
                Emotion.Confused => FrogMood.Reflective,
                Emotion.Numb => FrogMood.Reflective,
                Emotion.NeedingComfort => FrogMood.Sad,
                Emotion.NeedingSpace => FrogMood.Reflective,

                _ => FrogMood.None
            };
        }
    }
}
