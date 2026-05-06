using PondLite.Api.DTOs.CheckIns;
using PondLite.Api.Models;
using PondLite.Api.Repositories;

namespace PondLite.Api.Services
{
    public class DailyCheckInService : IDailyCheckInService
    {
        private readonly IDailyCheckInRepository _dailyCheckInRepository;
        private readonly IRelationshipMemberRepository _relationshipMemberRepository;
        private readonly IFrogService _frogService;

        public DailyCheckInService(
            IDailyCheckInRepository dailyCheckInRepository,
            IRelationshipMemberRepository relationshipMemberRepository,
            IFrogService frogService)
        {
            _dailyCheckInRepository = dailyCheckInRepository;
            _relationshipMemberRepository = relationshipMemberRepository;
            _frogService = frogService;
        }

        public async Task<DailyCheckInResponse?> CreateDailyCheckInAsync(
            Guid userAccountId,
            Guid relationshipId,
            CreateDailyCheckInRequest request)
        {
            RelationshipMember? relationshipMember =
                _relationshipMemberRepository.GetByUserAccountId(userAccountId);

            if (relationshipMember == null ||
                relationshipMember.RelationshipId != relationshipId)
            {
                return null;
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            DailyCheckIn? existingCheckIn =
                _dailyCheckInRepository.GetByRelationshipUserAndDate(
                    relationshipId,
                    userAccountId,
                    today);

            if (existingCheckIn != null)
            {
                return null;
            }

            DailyCheckIn dailyCheckIn = new DailyCheckIn
            {
                RelationshipId = relationshipId,
                UserAccountId = userAccountId,
                CheckInDate = today,
                PrimaryEmotion = request.PrimaryEmotion,
                SecondaryEmotion = request.SecondaryEmotion,
                TertiaryEmotion = request.TertiaryEmotion,
                OptionalMessage = string.IsNullOrWhiteSpace(request.OptionalMessage)
                    ? null
                    : request.OptionalMessage.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dailyCheckInRepository.Add(dailyCheckIn);

            await _frogService.UpdateFrogAfterCheckInAsync(
                relationshipMember.RelationshipMemberId,
                request.PrimaryEmotion);

            return BuildDailyCheckInResponse(dailyCheckIn);
        }

        public Task<DailyCheckInResponse?> GetMyTodayCheckInAsync(
            Guid userAccountId,
            Guid relationshipId)
        {
            bool belongsToRelationship =
                _relationshipMemberRepository.UserBelongsToRelationship(
                    userAccountId,
                    relationshipId);

            if (!belongsToRelationship)
            {
                return Task.FromResult<DailyCheckInResponse?>(null);
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            DailyCheckIn? checkIn =
                _dailyCheckInRepository.GetByRelationshipUserAndDate(
                    relationshipId,
                    userAccountId,
                    today);

            if (checkIn == null)
            {
                return Task.FromResult<DailyCheckInResponse?>(null);
            }

            DailyCheckInResponse response = BuildDailyCheckInResponse(checkIn);

            return Task.FromResult<DailyCheckInResponse?>(response);
        }

        public Task<List<PondMateCheckInStatusResponse>?> GetTodayPondCheckInStatusAsync(
            Guid userAccountId,
            Guid relationshipId)
        {
            bool belongsToRelationship =
                _relationshipMemberRepository.UserBelongsToRelationship(
                    userAccountId,
                    relationshipId);

            if (!belongsToRelationship)
            {
                return Task.FromResult<List<PondMateCheckInStatusResponse>?>(null);
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            List<RelationshipMember> relationshipMembers =
                _relationshipMemberRepository.GetByRelationshipId(relationshipId);

            List<DailyCheckIn> todaysCheckIns =
                _dailyCheckInRepository.GetByRelationshipAndDate(
                    relationshipId,
                    today);

            List<PondMateCheckInStatusResponse> response =
                relationshipMembers
                    .Select(member =>
                    {
                        DailyCheckIn? checkIn = todaysCheckIns
                            .FirstOrDefault(checkIn =>
                                checkIn.UserAccountId == member.UserAccountId);

                        return new PondMateCheckInStatusResponse
                        {
                            UserAccountId = member.UserAccountId,
                            RelationshipMemberId = member.RelationshipMemberId,
                            DisplayName = member.DisplayName,
                            HasCheckedInToday = checkIn != null,
                            TodayCheckIn = checkIn == null
                                ? null
                                : BuildDailyCheckInResponse(checkIn)
                        };
                    })
                    .ToList();

            return Task.FromResult<List<PondMateCheckInStatusResponse>?>(response);
        }

        private static DailyCheckInResponse BuildDailyCheckInResponse(
            DailyCheckIn dailyCheckIn)
        {
            return new DailyCheckInResponse
            {
                DailyCheckInId = dailyCheckIn.DailyCheckInId,
                RelationshipId = dailyCheckIn.RelationshipId,
                UserAccountId = dailyCheckIn.UserAccountId,
                CheckInDate = dailyCheckIn.CheckInDate,
                PrimaryEmotion = dailyCheckIn.PrimaryEmotion.ToString(),
                SecondaryEmotion = dailyCheckIn.SecondaryEmotion?.ToString(),
                TertiaryEmotion = dailyCheckIn.TertiaryEmotion?.ToString(),
                OptionalMessage = dailyCheckIn.OptionalMessage,
                CreatedAt = dailyCheckIn.CreatedAt,
                UpdatedAt = dailyCheckIn.UpdatedAt
            };
        }
    }
}