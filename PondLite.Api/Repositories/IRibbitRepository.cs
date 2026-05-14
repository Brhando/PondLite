using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IRibbitRepository
    {
        void Add(Ribbit ribbit);

        Ribbit? GetById(Guid ribbitId);

        List<Ribbit> GetActiveRibbitsForUser(
            Guid relationshipId,
            Guid userAccountId);

        List<Ribbit> GetSentRibbitsForUser(
            Guid relationshipId,
            Guid userAccountId);

        void Update(Ribbit ribbit);
    }
}