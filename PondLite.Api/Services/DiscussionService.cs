using PondLite.Api.DTOs.Discussions;
using PondLite.Api.DTOs.Relationship;
using PondLite.Api.Models;
using PondLite.Api.Repositories;

namespace PondLite.Api.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly IDiscussionPromptRepository _discussionPromptRepository;
        private readonly IDiscussionResponseRepository _discussionResponseRepository;
        private readonly IRelationshipService _relationshipService;

        private static readonly List<string> _promptOptions = new()
        {
            "What is one small thing I did recently that made you feel loved?",
            "What is something you are looking forward to doing together?",
            "What is one way I can make your day easier this week?",
            "What is a little moment with me that you have been thinking about lately?",
            "What is something you appreciate about us right now?",
            "What is one small thing we could do soon to feel more connected?",
            "What is something you want more of in our relationship lately?",
            "What is one thing I do that makes you feel safe or understood?",
            "What is a simple date idea that sounds nice right now?",
            "What is something you are proud of us for?"
        };

        private static string GetStaticPromptForDate(
            Guid relationshipId,
            DateOnly today)
        {
            int seed = HashCode.Combine(
                relationshipId,
                today.Year,
                today.Month,
                today.Day);

            int index = Math.Abs(seed) % _promptOptions.Count;

            return _promptOptions[index];
        }

        public DiscussionService(
            IDiscussionPromptRepository discussionPromptRepository,
            IDiscussionResponseRepository discussionResponseRepository,
            IRelationshipService relationshipService)
        {
            _discussionPromptRepository = discussionPromptRepository;
            _discussionResponseRepository = discussionResponseRepository;
            _relationshipService = relationshipService;
        }

        public async Task<PondDiscussionStatusResponse?> GetTodayDiscussionAsync(
            Guid userAccountId)
        {
            RelationshipResponse? activeRelationship =
                await _relationshipService.GetActiveRelationshipForUserAsync(
                    userAccountId);

            if (activeRelationship == null)
            {
                return null;
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            DiscussionPrompt prompt =
                GetOrCreateTodayPrompt(
                    activeRelationship.RelationshipId,
                    today);

            List<DiscussionResponse> responses =
                _discussionResponseRepository.GetByPromptId(
                    prompt.DiscussionPromptId);

            return BuildPondDiscussionStatusResponse(
                prompt,
                responses,
                userAccountId);
        }

        public async Task<PondDiscussionStatusResponse?> SubmitTodayResponseAsync(
            Guid userAccountId,
            SubmitDiscussionResponseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ResponseText))
            {
                return null;
            }

            RelationshipResponse? activeRelationship =
                await _relationshipService.GetActiveRelationshipForUserAsync(
                    userAccountId);

            if (activeRelationship == null)
            {
                return null;
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            DiscussionPrompt prompt =
                GetOrCreateTodayPrompt(
                    activeRelationship.RelationshipId,
                    today);

            DiscussionResponse? existingResponse =
                _discussionResponseRepository.GetByPromptAndUser(
                    prompt.DiscussionPromptId,
                    userAccountId);

            if (existingResponse == null)
            {
                DiscussionResponse discussionResponse = new DiscussionResponse
                {
                    DiscussionPromptId = prompt.DiscussionPromptId,
                    UserAccountId = userAccountId,
                    ResponseText = request.ResponseText.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _discussionResponseRepository.Add(discussionResponse);
            }
            else
            {
                existingResponse.ResponseText = request.ResponseText.Trim();
                existingResponse.UpdatedAt = DateTime.UtcNow;

                _discussionResponseRepository.Update(existingResponse);
            }

            List<DiscussionResponse> responses =
                _discussionResponseRepository.GetByPromptId(
                    prompt.DiscussionPromptId);

            return BuildPondDiscussionStatusResponse(
                prompt,
                responses,
                userAccountId);
        }

        private DiscussionPrompt GetOrCreateTodayPrompt(
            Guid relationshipId,
            DateOnly today)
        {
            DiscussionPrompt? existingPrompt =
                _discussionPromptRepository.GetByRelationshipAndDate(
                    relationshipId,
                    today);

            if (existingPrompt != null)
            {
                return existingPrompt;
            }

            DiscussionPrompt newPrompt = new DiscussionPrompt
            {
                RelationshipId = relationshipId,
                PromptDate = today,
                PromptText = GetStaticPromptForDate(relationshipId, today),
                CreatedAt = DateTime.UtcNow
            };

            _discussionPromptRepository.Add(newPrompt);

            return newPrompt;
        }

        private static PondDiscussionStatusResponse BuildPondDiscussionStatusResponse(
            DiscussionPrompt prompt,
            List<DiscussionResponse> responses,
            Guid currentUserAccountId)
        {
            return new PondDiscussionStatusResponse
            {
                DiscussionPromptId = prompt.DiscussionPromptId,
                RelationshipId = prompt.RelationshipId,
                PromptText = prompt.PromptText,
                PromptDate = prompt.PromptDate,
                ResponseCount = responses.Count,
                ExpectedResponseCount = 2,
                HasCurrentUserResponded = responses.Any(response =>
                    response.UserAccountId == currentUserAccountId),
                Responses = responses
                    .Select(MapToResponse)
                    .ToList()
            };
        }

        private static DiscussionResponseResponse MapToResponse(
            DiscussionResponse response)
        {
            return new DiscussionResponseResponse
            {
                DiscussionResponseId = response.DiscussionResponseId,
                DiscussionPromptId = response.DiscussionPromptId,
                UserAccountId = response.UserAccountId,
                ResponseText = response.ResponseText,
                CreatedAt = response.CreatedAt,
                UpdatedAt = response.UpdatedAt
            };
        }
    }
}