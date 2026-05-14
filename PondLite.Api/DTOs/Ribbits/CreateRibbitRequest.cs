using PondLite.Api.Models.Enums;

namespace PondLite.Api.DTOs.Ribbits
{
    public class CreateRibbitRequest
    {
        public Guid ReceiverUserId { get; set; }

        public RibbitType Type { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime? DeliveryTime { get; set; }
    }
}