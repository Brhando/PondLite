using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfDailyCheckInRepository : IDailyCheckInRepository
    {
        private readonly PondLiteDbContext _context;

        public EfDailyCheckInRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(DailyCheckIn dailyCheckIn)
        {
            _context.DailyCheckIns.Add(dailyCheckIn);
            _context.SaveChanges();
        }

        public DailyCheckIn? GetByRelationshipUserAndDate(
            Guid relationshipId,
            Guid userAccountId,
            DateOnly checkInDate)
        {
            return _context.DailyCheckIns
                .FirstOrDefault(checkIn =>
                    checkIn.RelationshipId == relationshipId &&
                    checkIn.UserAccountId == userAccountId &&
                    checkIn.CheckInDate == checkInDate);
        }

        public List<DailyCheckIn> GetByRelationshipAndDate(
            Guid relationshipId,
            DateOnly checkInDate)
        {
            return _context.DailyCheckIns
                .Where(checkIn =>
                    checkIn.RelationshipId == relationshipId &&
                    checkIn.CheckInDate == checkInDate)
                .ToList();
        }
    }
}