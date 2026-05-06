using PondLite.Api.DTOs.CheckIns;

namespace PondLite.Api.Services
{
    public interface IDailyCheckInService
    {
        Task<DailyCheckInResponse?> CreateDailyCheckInAsync(
            Guid userAccountId,
            Guid relationshipId,
            CreateDailyCheckInRequest request);

        Task<DailyCheckInResponse?> GetMyTodayCheckInAsync(
            Guid userAccountId,
            Guid relationshipId);

        Task<List<PondMateCheckInStatusResponse>?> GetTodayPondCheckInStatusAsync(
            Guid userAccountId,
            Guid relationshipId);
    }
}