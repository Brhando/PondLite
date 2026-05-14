using Microsoft.AspNetCore.Mvc;
using PondLite.Api.DTOs.Ribbits;
using PondLite.Api.Models;
using PondLite.Api.Services;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/ribbit")]
    public class RibbitController : ControllerBase
    {
        private readonly IRibbitService _ribbitService;
        private readonly IAuthService _authService;

        public RibbitController(
            IRibbitService ribbitService,
            IAuthService authService)
        {
            _ribbitService = ribbitService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRibbit(
            [FromBody] CreateRibbitRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RibbitResponse? response =
                await _ribbitService.CreateRibbitAsync(
                    user.AccountId,
                    request);

            if (response == null)
            {
                return BadRequest("Unable to create Ribbit. Make sure the receiver belongs to your Pond and the message is valid.");
            }

            return Ok(response);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRibbits()
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            List<RibbitResponse> response =
                await _ribbitService.GetActiveRibbitsForUserAsync(
                    user.AccountId);

            return Ok(response);
        }

        [HttpGet("sent")]
        public async Task<IActionResult> GetSentRibbits()
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            List<RibbitResponse> response =
                await _ribbitService.GetSentRibbitsForUserAsync(
                    user.AccountId);

            return Ok(response);
        }

        [HttpPatch("{ribbitId}/acknowledge")]
        public async Task<IActionResult> AcknowledgeRibbit(
            Guid ribbitId,
            [FromBody] AcknowledgeRibbitRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RibbitResponse? response =
                await _ribbitService.AcknowledgeRibbitAsync(
                    user.AccountId,
                    ribbitId,
                    request);

            if (response == null)
            {
                return BadRequest("Unable to acknowledge Ribbit.");
            }

            return Ok(response);
        }

        [HttpPatch("{ribbitId}/complete")]
        public async Task<IActionResult> CompleteRibbit(Guid ribbitId)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RibbitResponse? response =
                await _ribbitService.CompleteRibbitAsync(
                    user.AccountId,
                    ribbitId);

            if (response == null)
            {
                return BadRequest("Unable to complete Ribbit.");
            }

            return Ok(response);
        }

        [HttpPatch("{ribbitId}/decline")]
        public async Task<IActionResult> DeclineRibbit(Guid ribbitId)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RibbitResponse? response =
                await _ribbitService.DeclineRibbitAsync(
                    user.AccountId,
                    ribbitId);

            if (response == null)
            {
                return BadRequest("Unable to decline Ribbit.");
            }

            return Ok(response);
        }

        [HttpPatch("{ribbitId}/cancel")]
        public async Task<IActionResult> CancelRibbit(Guid ribbitId)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RibbitResponse? response =
                await _ribbitService.CancelRibbitAsync(
                    user.AccountId,
                    ribbitId);

            if (response == null)
            {
                return BadRequest("Unable to cancel Ribbit.");
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