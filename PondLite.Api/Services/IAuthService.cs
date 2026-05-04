using PondLite.Api.DTOs;
using PondLite.Api.Models;

namespace PondLite.Api.Services
{
    public interface IAuthService
    {
        AuthResponse CreateAccount(CreateAccountRequest request);

        AuthResponse Login(LoginRequest request);

        bool Logout(string tokenValue);

        UserAccount? GetUserFromToken(string tokenValue);
    }
}