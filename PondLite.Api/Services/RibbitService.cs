using PondLite.Api.DTOs.Relationship;
using PondLite.Api.DTOs.Ribbits;
using PondLite.Api.Models;
using PondLite.Api.Models.Enums;
using PondLite.Api.Repositories;

namespace PondLite.Api.Services
{
    public class RibbitService : IRibbitService
    {
        private readonly IRibbitRepository _ribbitRepository;
        private readonly IRelationshipService _relationshipService;

        public RibbitService(
            IRibbitRepository ribbitRepository,
            IRelationshipService relationshipService)
        {
            _ribbitRepository = ribbitRepository;
            _relationshipService = relationshipService;
        }

        public async Task<RibbitResponse?> CreateRibbitAsync(
            Guid senderUserId,
            CreateRibbitRequest request)
        {
            RelationshipResponse? activeRelationship =
                await _relationshipService.GetActiveRelationshipForUserAsync(senderUserId);

            if (activeRelationship == null)
            {
                return null;
            }

            bool receiverBelongsToRelationship =
                await _relationshipService.UserBelongsToRelationshipAsync(
                    request.ReceiverUserId,
                    activeRelationship.RelationshipId);

            if (!receiverBelongsToRelationship)
            {
                return null;
            }

            if (request.ReceiverUserId == senderUserId)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return null;
            }

            Ribbit ribbit = new Ribbit
            {
                RelationshipId = activeRelationship.RelationshipId,
                SenderUserId = senderUserId,
                ReceiverUserId = request.ReceiverUserId,
                Type = request.Type,
                Status = RibbitStatus.Sent,
                Message = request.Message.Trim(),
                DeliveryTime = request.DeliveryTime,
                CreatedAt = DateTime.UtcNow
            };

            _ribbitRepository.Add(ribbit);

            return MapToResponse(ribbit);
        }

        public async Task<List<RibbitResponse>> GetActiveRibbitsForUserAsync(
            Guid userAccountId)
        {
            RelationshipResponse? activeRelationship =
                await _relationshipService.GetActiveRelationshipForUserAsync(userAccountId);

            if (activeRelationship == null)
            {
                return new List<RibbitResponse>();
            }

            List<Ribbit> ribbits =
                _ribbitRepository.GetActiveRibbitsForUser(
                    activeRelationship.RelationshipId,
                    userAccountId);

            return ribbits
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<List<RibbitResponse>> GetSentRibbitsForUserAsync(
            Guid userAccountId)
        {
            RelationshipResponse? activeRelationship =
                await _relationshipService.GetActiveRelationshipForUserAsync(userAccountId);

            if (activeRelationship == null)
            {
                return new List<RibbitResponse>();
            }

            List<Ribbit> ribbits =
                _ribbitRepository.GetSentRibbitsForUser(
                    activeRelationship.RelationshipId,
                    userAccountId);

            return ribbits
                .Select(MapToResponse)
                .ToList();
        }

        public Task<RibbitResponse?> AcknowledgeRibbitAsync(
            Guid userAccountId,
            Guid ribbitId,
            AcknowledgeRibbitRequest request)
        {
            Ribbit? ribbit = _ribbitRepository.GetById(ribbitId);

            if (ribbit == null)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.ReceiverUserId != userAccountId)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.Status == RibbitStatus.Cancelled ||
                ribbit.Status == RibbitStatus.Completed ||
                ribbit.Status == RibbitStatus.Declined)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            ribbit.Status = RibbitStatus.Acknowledged;
            ribbit.AcknowledgementEmoji = string.IsNullOrWhiteSpace(request.AcknowledgementEmoji)
                ? null
                : request.AcknowledgementEmoji.Trim();
            ribbit.AcknowledgedAt = DateTime.UtcNow;

            _ribbitRepository.Update(ribbit);

            return Task.FromResult<RibbitResponse?>(MapToResponse(ribbit));
        }

        public Task<RibbitResponse?> CompleteRibbitAsync(
            Guid userAccountId,
            Guid ribbitId)
        {
            Ribbit? ribbit = _ribbitRepository.GetById(ribbitId);

            if (ribbit == null)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.ReceiverUserId != userAccountId)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.Status == RibbitStatus.Cancelled ||
                ribbit.Status == RibbitStatus.Completed ||
                ribbit.Status == RibbitStatus.Declined)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            ribbit.Status = RibbitStatus.Completed;
            ribbit.CompletedAt = DateTime.UtcNow;

            _ribbitRepository.Update(ribbit);

            return Task.FromResult<RibbitResponse?>(MapToResponse(ribbit));
        }

        public Task<RibbitResponse?> DeclineRibbitAsync(
            Guid userAccountId,
            Guid ribbitId)
        {
            Ribbit? ribbit = _ribbitRepository.GetById(ribbitId);

            if (ribbit == null)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.ReceiverUserId != userAccountId)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.Status == RibbitStatus.Cancelled ||
                ribbit.Status == RibbitStatus.Completed ||
                ribbit.Status == RibbitStatus.Declined)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            ribbit.Status = RibbitStatus.Declined;
            ribbit.DeclinedAt = DateTime.UtcNow;

            _ribbitRepository.Update(ribbit);

            return Task.FromResult<RibbitResponse?>(MapToResponse(ribbit));
        }

        public Task<RibbitResponse?> CancelRibbitAsync(
            Guid userAccountId,
            Guid ribbitId)
        {
            Ribbit? ribbit = _ribbitRepository.GetById(ribbitId);

            if (ribbit == null)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.SenderUserId != userAccountId)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            if (ribbit.Status == RibbitStatus.Completed ||
                ribbit.Status == RibbitStatus.Declined ||
                ribbit.Status == RibbitStatus.Cancelled)
            {
                return Task.FromResult<RibbitResponse?>(null);
            }

            ribbit.Status = RibbitStatus.Cancelled;
            ribbit.CancelledAt = DateTime.UtcNow;

            _ribbitRepository.Update(ribbit);

            return Task.FromResult<RibbitResponse?>(MapToResponse(ribbit));
        }

        private static RibbitResponse MapToResponse(Ribbit ribbit)
        {
            return new RibbitResponse
            {
                RibbitId = ribbit.RibbitId,
                RelationshipId = ribbit.RelationshipId,
                SenderUserId = ribbit.SenderUserId,
                ReceiverUserId = ribbit.ReceiverUserId,
                Type = ribbit.Type,
                Status = ribbit.Status,
                Message = ribbit.Message,
                AcknowledgementEmoji = ribbit.AcknowledgementEmoji,
                DeliveryTime = ribbit.DeliveryTime,
                CreatedAt = ribbit.CreatedAt,
                AcknowledgedAt = ribbit.AcknowledgedAt,
                CompletedAt = ribbit.CompletedAt,
                DeclinedAt = ribbit.DeclinedAt,
                CancelledAt = ribbit.CancelledAt
            };
        }
    }
}