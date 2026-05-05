using PondLite.Api.Data;
using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class EfUserAccountRepository : IUserAccountRepository
    {
        private readonly PondLiteDbContext _context;

        public EfUserAccountRepository(PondLiteDbContext context)
        {
            _context = context;
        }

        public void Add(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            _context.SaveChanges();
        }

        public UserAccount? GetByUsernameOrEmail(string usernameOrEmail)
        {
            return _context.UserAccounts.FirstOrDefault(u =>
                u.Username.ToLower() == usernameOrEmail.ToLower() ||
                u.Email.ToLower() == usernameOrEmail.ToLower());
        }

        public UserAccount? GetById(Guid userAccountId)
        {
            return _context.UserAccounts.FirstOrDefault(u => u.AccountId == userAccountId);
        }

        public bool UsernameExists(string username)
        {
            return _context.UserAccounts.Any(u =>
                u.Username.ToLower() == username.ToLower());
        }

        public bool EmailExists(string email)
        {
            return _context.UserAccounts.Any(u =>
                u.Email.ToLower() == email.ToLower());
        }
    }
}