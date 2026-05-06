using PondLite.Api.DTOs.Frogs;
using PondLite.Api.Models.Enums;

namespace PondLite.Api.Services
{
    public interface IFrogService
    {
        Task<FrogResponse> CreateDefaultFrogForRelationshipMemberAsync(
            Guid relationshipMemberId);

        Task<FrogResponse?> CreateMyFrogAsync(Guid userAccountId);

        Task<FrogResponse?> GetMyFrogAsync(Guid userAccountId);

        Task<FrogResponse?> UpdateMyFrogAsync(
            Guid userAccountId,
            UpdateFrogRequest request);

        Task<FrogResponse?> UpdateFrogAfterCheckInAsync(
            Guid relationshipMemberId,
            Emotion primaryEmotion);
    }
}
