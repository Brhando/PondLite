using PondLite.Api.DTOs.Frogs;

namespace PondLite.Api.Services
{
    public interface IFrogService
    {
        Task<FrogResponse?> GetMyFrogAsync(Guid userAccountId);
    }
}
