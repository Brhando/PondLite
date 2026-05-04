using Microsoft.AspNetCore.Identity;
using PondLite.Api.DTOs;
using PondLite.Api.Models;

namespace PondLite.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly List<UserAccount> _users = new();
        private readonly List<AuthToken> _tokens = new();

        private readonly PasswordHasher<UserAccount> _passwordHasher = new();

        public AuthResponse CreateAccount(CreateAccountRequest request)
        {
            UserAccount user = new UserAccount
            {
                Username = request.Username,
                Email = request.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _users.Add(user);

            AuthToken token = CreateTokenForUser(user);

            return BuildAuthResponse(user, token);
        }

        public AuthResponse Login(LoginRequest request)
        {
            UserAccount? user = _users.FirstOrDefault(u =>
                u.Username == request.UsernameOrEmail ||
                u.Email == request.UsernameOrEmail);

            if (user == null)
            {
                throw new Exception("Account not found.");
            }

            PasswordVerificationResult passwordResult =
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid password.");
            }

            AuthToken token = CreateTokenForUser(user);

            return BuildAuthResponse(user, token);
        }

        public bool Logout(string tokenValue)
        {
            AuthToken? token = _tokens.FirstOrDefault(t => t.TokenValue == tokenValue);

            if (token == null)
            {
                return false;
            }

            token.IsActive = false;
            return true;
        }

        public UserAccount? GetUserFromToken(string tokenValue)
        {
            AuthToken? token = _tokens.FirstOrDefault(t =>
                t.TokenValue == tokenValue &&
                t.IsActive &&
                t.ExpiresAt > DateTime.UtcNow);

            if (token == null)
            {
                return null;
            }

            return _users.FirstOrDefault(u => u.AccountId == token.UserAccountId);
        }

        private AuthToken CreateTokenForUser(UserAccount user)
        {
            AuthToken token = new AuthToken
            {
                UserAccountId = user.AccountId,
                TokenValue = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                IsActive = true
            };

            _tokens.Add(token);

            return token;
        }

        private AuthResponse BuildAuthResponse(UserAccount user, AuthToken token)
        {
            return new AuthResponse
            {
                UserAccountId = user.AccountId,
                Username = user.Username,
                Email = user.Email,
                Token = token.TokenValue,
                ExpiresAt = token.ExpiresAt
            };
        }
    }
}