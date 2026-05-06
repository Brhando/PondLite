using Microsoft.AspNetCore.Mvc;
using PondLite.Api.DTOs.CheckIns;
using PondLite.Api.Models;
using PondLite.Api.Services;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/relationships/{relationshipId:guid}/checkins")]
    public class DailyCheckInController : ControllerBase
    {
        private readonly IDailyCheckInService _dailyCheckInService;
        private readonly IAuthService _authService;

        public DailyCheckInController(
            IDailyCheckInService dailyCheckInService,
            IAuthService authService)
        {
            _dailyCheckInService = dailyCheckInService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDailyCheckIn(
            Guid relationshipId,
            [FromBody] CreateDailyCheckInRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            if (request.PrimaryEmotion == Models.Enums.Emotion.None)
            {
                return BadRequest("Primary emotion is required.");
            }

            DailyCheckInResponse? response =
                await _dailyCheckInService.CreateDailyCheckInAsync(
                    user.AccountId,
                    relationshipId,
                    request);

            if (response == null)
            {
                return BadRequest(
                    "Unable to create check-in. You may not belong to this Pond, or you may have already checked in today.");
            }

            return Ok(response);
        }

        [HttpGet("today/me")]
        public async Task<IActionResult> GetMyTodayCheckIn(Guid relationshipId)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            DailyCheckInResponse? response =
                await _dailyCheckInService.GetMyTodayCheckInAsync(
                    user.AccountId,
                    relationshipId);

            if (response == null)
            {
                return NotFound(
                    "No check-in found for today, or you do not have access to this Pond.");
            }

            return Ok(response);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayPondCheckInStatus(
            Guid relationshipId)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            List<PondMateCheckInStatusResponse>? response =
                await _dailyCheckInService.GetTodayPondCheckInStatusAsync(
                    user.AccountId,
                    relationshipId);

            if (response == null)
            {
                return NotFound(
                    "Pond not found, or you do not have access to this Pond.");
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