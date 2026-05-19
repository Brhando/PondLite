using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IDiscussionResponseRepository
    {
        void Add(DiscussionResponse discussionResponse);

        DiscussionResponse? GetByPromptAndUser(
            Guid discussionPromptId,
            Guid userAccountId);

        List<DiscussionResponse> GetByPromptId(Guid discussionPromptId);

        void Update(DiscussionResponse discussionResponse);
    }
}