using Microsoft.EntityFrameworkCore;
using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfDiscussionPromptRepository : IDiscussionPromptRepository
    {
        private readonly PondLiteDbContext _context;

        public EfDiscussionPromptRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(DiscussionPrompt discussionPrompt)
        {
            _context.DiscussionPrompts.Add(discussionPrompt);
            _context.SaveChanges();
        }

        public DiscussionPrompt? GetByRelationshipAndDate(
            Guid relationshipId,
            DateOnly promptDate)
        {
            return _context.DiscussionPrompts
                .Include(prompt => prompt.Responses)
                .FirstOrDefault(prompt =>
                    prompt.RelationshipId == relationshipId &&
                    prompt.PromptDate == promptDate);
        }

        public DiscussionPrompt? GetById(Guid discussionPromptId)
        {
            return _context.DiscussionPrompts
                .Include(prompt => prompt.Responses)
                .FirstOrDefault(prompt =>
                    prompt.DiscussionPromptId == discussionPromptId);
        }

        public void Update(DiscussionPrompt discussionPrompt)
        {
            _context.DiscussionPrompts.Update(discussionPrompt);
            _context.SaveChanges();
        }
    }
}