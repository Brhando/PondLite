using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IDailyCheckInRepository
    {
        void Add(DailyCheckIn dailyCheckIn);

        DailyCheckIn? GetByRelationshipUserAndDate(
            Guid relationshipId,
            Guid userAccountId,
            DateOnly checkInDate);

        List<DailyCheckIn> GetByRelationshipAndDate(
            Guid relationshipId,
            DateOnly checkInDate);
    }
}