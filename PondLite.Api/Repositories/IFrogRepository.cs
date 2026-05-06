using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IFrogRepository
    {
        void Add(Frog frog);

        Frog? GetByRelationshipMemberId(Guid relationshipMemberId);

        void Update(Frog frog);
    }
}
