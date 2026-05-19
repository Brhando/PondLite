using PondLite.Api.DTOs.Discussions;

namespace PondLite.Api.Services
{
    public interface IDiscussionService
    {
        Task<PondDiscussionStatusResponse?> GetTodayDiscussionAsync(
            Guid userAccountId);

        Task<PondDiscussionStatusResponse?> SubmitTodayResponseAsync(
            Guid userAccountId,
            SubmitDiscussionResponseRequest request);
    }
}