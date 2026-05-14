using PondLite.Api.Data;
using PondLite.Api.Models;
using PondLite.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace PondLite.Api.Repositories
{
    public class EfRibbitRepository : IRibbitRepository
    {
        private readonly PondLiteDbContext _dbContext;

        public EfRibbitRepository(PondLiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Ribbit ribbit)
        {
            _dbContext.Ribbits.Add(ribbit);
            _dbContext.SaveChanges();
        }

        public Ribbit? GetById(Guid ribbitId)
        {
            return _dbContext.Ribbits
                .Include(ribbit => ribbit.Relationship)
                .Include(ribbit => ribbit.SenderUser)
                .Include(ribbit => ribbit.ReceiverUser)
                .FirstOrDefault(ribbit => ribbit.RibbitId == ribbitId);
        }

        public List<Ribbit> GetActiveRibbitsForUser(
            Guid relationshipId,
            Guid userAccountId)
        {
            return _dbContext.Ribbits
                .Where(ribbit =>
                    ribbit.RelationshipId == relationshipId &&
                    ribbit.ReceiverUserId == userAccountId &&
                    ribbit.Status != RibbitStatus.Cancelled &&
                    ribbit.Status != RibbitStatus.Completed &&
                    ribbit.Status != RibbitStatus.Declined)
                .OrderByDescending(ribbit => ribbit.CreatedAt)
                .ToList();
        }

        public List<Ribbit> GetSentRibbitsForUser(
            Guid relationshipId,
            Guid userAccountId)
        {
            return _dbContext.Ribbits
                .Where(ribbit =>
                    ribbit.RelationshipId == relationshipId &&
                    ribbit.SenderUserId == userAccountId)
                .OrderByDescending(ribbit => ribbit.CreatedAt)
                .ToList();
        }

        public void Update(Ribbit ribbit)
        {
            _dbContext.Ribbits.Update(ribbit);
            _dbContext.SaveChanges();
        }
    }
}