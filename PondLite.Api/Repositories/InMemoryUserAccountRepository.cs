using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public class InMemoryUserAccountRepository : IUserAccountRepository
    {
        private readonly List<UserAccount> _users = new();

        public void Add(UserAccount user)
        {
            _users.Add(user);
        }

        public UserAccount? GetByUsernameOrEmail(string usernameOrEmail)
        {
            return _users.FirstOrDefault(u =>
                u.Username.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase));
        }

        public UserAccount? GetById(Guid userAccountId)
        {
            return _users.FirstOrDefault(u => u.AccountId == userAccountId);
        }

        public bool UsernameExists(string username)
        {
            return _users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public bool EmailExists(string email)
        {
            return _users.Any(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}