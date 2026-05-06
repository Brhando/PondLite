using PondLite.Api.DTOs.Frogs;

namespace PondLite.Api.Services
{
    public interface IFrogService
    {
        Task<FrogResponse> CreateDefaultFrogForRelationshipMemberAsync(
            Guid relationshipMemberId);

        Task<FrogResponse?> GetMyFrogAsync(Guid userAccountId);

        Task<FrogResponse?> UpdateMyFrogAsync(
            Guid userAccountId,
            UpdateFrogRequest request);
    }
}
