using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class InMemoryAuthTokenRepository : IAuthTokenRepository
    {
        private readonly List<AuthToken> _tokens = new();

        public void Add(AuthToken token)
        {
            _tokens.Add(token);
        }

        public AuthToken? GetByTokenValue(string tokenValue)
        {
            return _tokens.FirstOrDefault(t => t.TokenValue == tokenValue);
        }

        public AuthToken? GetActiveToken(string tokenValue)
        {
            return _tokens.FirstOrDefault(t =>
                t.TokenValue == tokenValue &&
                t.IsActive &&
                t.ExpiresAt > DateTime.UtcNow);
        }

        public void Update(AuthToken token)
        {
            // No action needed for in-memory storage right now.
            // Since AuthToken is a reference type, changes to the object are already reflected in the list.
        }
    }
}