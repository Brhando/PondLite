namespace PondLite.Api.DTOs.CheckIns
{
    public class PondMateCheckInStatusResponse
    {
        public Guid UserAccountId { get; set; }

        public Guid RelationshipMemberId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public bool HasCheckedInToday { get; set; }

        public DailyCheckInResponse? TodayCheckIn { get; set; }
    }
}