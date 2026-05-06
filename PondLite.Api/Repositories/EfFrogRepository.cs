using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfFrogRepository : IFrogRepository
    {
        private readonly PondLiteDbContext _context;

        public EfFrogRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(Frog frog)
        {
            _context.Frogs.Add(frog);
            _context.SaveChanges();
        }

        public Frog? GetByRelationshipMemberId(Guid relationshipMemberId)
        {
            return _context.Frogs
                .FirstOrDefault(frog =>
                    frog.RelationshipMemberId == relationshipMemberId);
        }
    }
}
