using Microsoft.AspNetCore.Mvc;
using PondLite.Api.DTOs.Discussions;
using PondLite.Api.Models;
using PondLite.Api.Services;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/discussions")]
    public class DiscussionController : ControllerBase
    {
        private readonly IDiscussionService _discussionService;
        private readonly IAuthService _authService;

        public DiscussionController(
            IDiscussionService discussionService,
            IAuthService authService)
        {
            _discussionService = discussionService;
            _authService = authService;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayDiscussion()
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            PondDiscussionStatusResponse? response =
                await _discussionService.GetTodayDiscussionAsync(
                    user.AccountId);

            if (response == null)
            {
                return NotFound("No active Pond found for this user.");
            }

            return Ok(response);
        }

        [HttpPost("today/response")]
        public async Task<IActionResult> SubmitTodayResponse(
            [FromBody] SubmitDiscussionResponseRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            PondDiscussionStatusResponse? response =
                await _discussionService.SubmitTodayResponseAsync(
                    user.AccountId,
                    request);

            if (response == null)
            {
                return BadRequest(
                    "Unable to submit discussion response. Make sure you belong to a Pond and the response is not empty.");
            }

            return Ok(response);
        }

        private UserAccount? GetUserFromBearerToken()
        {
            string? authHeader = Request.Headers.Authorization.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) ||
                !authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            string token = authHeader["Bearer ".Length..].Trim();

            return _authService.GetUserFromToken(token);
        }
    }
}