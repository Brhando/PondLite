namespace PondLite.Api.DTOs.Discussions
{
    public class DiscussionResponseResponse
    {
        public Guid DiscussionResponseId { get; set; }

        public Guid DiscussionPromptId { get; set; }

        public Guid UserAccountId { get; set; }

        public string ResponseText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}