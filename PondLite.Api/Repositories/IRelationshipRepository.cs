using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IRelationshipRepository
    {
        void Add(Relationship relationship);

        Relationship? GetById(Guid relationshipId);

        void Update(Relationship relationship);
    }
}