using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IAuthTokenRepository
    {
        void Add(AuthToken token);

        AuthToken? GetByTokenValue(string tokenValue);

        AuthToken? GetActiveToken(string tokenValue);

        void Update(AuthToken token);
    }
}