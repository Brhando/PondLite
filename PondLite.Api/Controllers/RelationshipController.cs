using Microsoft.AspNetCore.Mvc;
using PondLite.Api.DTOs.Relationship;
using PondLite.Api.Models;
using PondLite.Api.Services;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/relationship")]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;
        private readonly IAuthService _authService;

        public RelationshipController(
            IRelationshipService relationshipService,
            IAuthService authService)
        {
            _relationshipService = relationshipService;
            _authService = authService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRelationship(
            [FromBody] CreateRelationshipRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RelationshipResponse? response =
                await _relationshipService.CreateRelationshipAsync(
                    user.AccountId,
                    request);

            if (response == null)
            {
                return BadRequest("User already belongs to an active Pond.");
            }

            return Ok(response);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyRelationship()
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RelationshipResponse? response =
                await _relationshipService.GetActiveRelationshipForUserAsync(
                    user.AccountId);

            if (response == null)
            {
                return NotFound("No active Pond found for this user.");
            }

            return Ok(response);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinRelationship(
        [FromBody] JoinRelationshipRequest request)
        {
            UserAccount? user = GetUserFromBearerToken();

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            RelationshipResponse? response =
                await _relationshipService.JoinRelationshipAsync(
                    user.AccountId,
                    request);

            if (response == null)
            {
                return BadRequest("Unable to join Pond. The Pond may not exist, may already be full, or you may already belong to a Pond.");
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