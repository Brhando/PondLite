using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfRelationshipRepository : IRelationshipRepository
    {
        private readonly PondLiteDbContext _context;

        public EfRelationshipRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(Relationship relationship)
        {
            _context.Relationships.Add(relationship);
            _context.SaveChanges();
        }

        public Relationship? GetById(Guid relationshipId)
        {
            return _context.Relationships
                .FirstOrDefault(r => r.RelationshipId == relationshipId);
        }

        public void Update(Relationship relationship)
        {
            _context.Relationships.Update(relationship);
            _context.SaveChanges();
        }
    }
}