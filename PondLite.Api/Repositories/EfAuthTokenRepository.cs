using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfAuthTokenRepository : IAuthTokenRepository
    {
        private readonly PondLiteDbContext _context;

        public EfAuthTokenRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(AuthToken token)
        {
            _context.AuthTokens.Add(token);
            _context.SaveChanges();
        }

        public AuthToken? GetByTokenValue(string tokenValue)
        {
            return _context.AuthTokens.FirstOrDefault(t => t.TokenValue == tokenValue);
        }

        public AuthToken? GetActiveToken(string tokenValue)
        {
            return _context.AuthTokens.FirstOrDefault(t =>
                t.TokenValue == tokenValue &&
                t.IsActive &&
                t.ExpiresAt > DateTime.UtcNow);
        }

        public void Update(AuthToken token)
        {
            _context.AuthTokens.Update(token);
            _context.SaveChanges();
        }
    }
}