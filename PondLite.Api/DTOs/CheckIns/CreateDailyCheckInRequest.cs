using PondLite.Api.Models.Enums;

namespace PondLite.Api.DTOs.CheckIns
{
    public class CreateDailyCheckInRequest
    {
        public Emotion PrimaryEmotion { get; set; }

        public Emotion? SecondaryEmotion { get; set; }

        public Emotion? TertiaryEmotion { get; set; }

        public string? OptionalMessage { get; set; }
    }
}