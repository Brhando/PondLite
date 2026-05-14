using PondLite.Api.DTOs.Ribbits;

namespace PondLite.Api.Services
{
    public interface IRibbitService
    {
        Task<RibbitResponse?> CreateRibbitAsync(
            Guid senderUserId,
            CreateRibbitRequest request);

        Task<List<RibbitResponse>> GetActiveRibbitsForUserAsync(
            Guid userAccountId);

        Task<List<RibbitResponse>> GetSentRibbitsForUserAsync(
            Guid userAccountId);

        Task<RibbitResponse?> AcknowledgeRibbitAsync(
            Guid userAccountId,
            Guid ribbitId,
            AcknowledgeRibbitRequest request);

        Task<RibbitResponse?> CompleteRibbitAsync(
            Guid userAccountId,
            Guid ribbitId);

        Task<RibbitResponse?> DeclineRibbitAsync(
            Guid userAccountId,
            Guid ribbitId);

        Task<RibbitResponse?> CancelRibbitAsync(
            Guid userAccountId,
            Guid ribbitId);
    }
}