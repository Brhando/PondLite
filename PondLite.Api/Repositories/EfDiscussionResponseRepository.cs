using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfDiscussionResponseRepository : IDiscussionResponseRepository
    {
        private readonly PondLiteDbContext _context;

        public EfDiscussionResponseRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(DiscussionResponse discussionResponse)
        {
            _context.DiscussionResponses.Add(discussionResponse);
            _context.SaveChanges();
        }

        public DiscussionResponse? GetByPromptAndUser(
            Guid discussionPromptId,
            Guid userAccountId)
        {
            return _context.DiscussionResponses
                .FirstOrDefault(response =>
                    response.DiscussionPromptId == discussionPromptId &&
                    response.UserAccountId == userAccountId);
        }

        public List<DiscussionResponse> GetByPromptId(Guid discussionPromptId)
        {
            return _context.DiscussionResponses
                .Where(response =>
                    response.DiscussionPromptId == discussionPromptId)
                .ToList();
        }

        public void Update(DiscussionResponse discussionResponse)
        {
            _context.DiscussionResponses.Update(discussionResponse);
            _context.SaveChanges();
        }
    }
}