using Microsoft.AspNetCore.Mvc;
using PondLite.Api.DTOs;
using PondLite.Api.Services;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        private string? GetBearerToken()
        {
            string? authHeader = Request.Headers.Authorization;

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return null;
            }

            if (!authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            return authHeader["Bearer ".Length..];
        }

        [HttpPost("register")]
        public IActionResult Register(CreateAccountRequest request)
        {
            try
            {
                AuthResponse response = _authService.CreateAccount(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                AuthResponse response = _authService.Login(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string? tokenValue = GetBearerToken();

            if (tokenValue == null)
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header." });
            }

            bool loggedOut = _authService.Logout(tokenValue);

            if (!loggedOut)
            {
                return BadRequest(new { message = "Token not found or already invalid." });
            }

            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            string? tokenValue = GetBearerToken();

            if (tokenValue == null)
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header." });
            }

            var user = _authService.GetUserFromToken(tokenValue);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid or expired token." });
            }

            return Ok(new
            {
                userAccountId = user.AccountId,
                username = user.Username,
                email = user.Email
            });
        }
    }
}