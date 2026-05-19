using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IDiscussionPromptRepository
    {
        void Add(DiscussionPrompt discussionPrompt);

        DiscussionPrompt? GetByRelationshipAndDate(
            Guid relationshipId,
            DateOnly promptDate);

        DiscussionPrompt? GetById(Guid discussionPromptId);

        void Update(DiscussionPrompt discussionPrompt);
    }
}