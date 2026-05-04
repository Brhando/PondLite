using PondLite.Api.Models;

namespace PondLite.Api.Repositories
{
    public interface IUserAccountRepository
    {
        void Add(UserAccount user);

        UserAccount? GetByUsernameOrEmail(string usernameOrEmail);

        UserAccount? GetById(Guid userAccountId);

        bool UsernameExists(string username);

        bool EmailExists(string email);
    }
}