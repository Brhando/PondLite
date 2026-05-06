using Microsoft.AspNetCore.Mvc;
using PondLite.Api.DTOs.Frogs;
using PondLite.Api.Models;
using PondLite.Api.Services;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/frog")]
    public class FrogController : ControllerBase
    {
        private readonly IFrogService _frogService;
        private readonly IAuthService _authService;

        public FrogController(
            IFrogService frogService,
            IAuthService authService)
        {
            _frogService = frogService;
            _authService = authService;
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyFrog()
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            FrogResponse? response =
                await _frogService.GetMyFrogAsync(user.AccountId);

            if (response == null)
            {
                return NotFound("No Frog found for this PondMate.");
            }

            return Ok(response);
        }

        [HttpPut("mine")]
        public async Task<IActionResult> UpdateMyFrog(
            [FromBody] UpdateFrogRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Frog name is required.");
            }

            FrogResponse? response =
                await _frogService.UpdateMyFrogAsync(
                    user.AccountId,
                    request);

            if (response == null)
            {
                return NotFound("No Frog found for this PondMate.");
            }

            return Ok(response);
        }

        private UserAccount? GetUserFromBearerToken()
        {
            string authHeader = Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authHeader) ||
                !authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            string tokenValue = authHeader["Bearer ".Length..].Trim();

            return _authService.GetUserFromToken(tokenValue);
        }
    }
}
