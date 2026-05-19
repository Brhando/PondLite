namespace PondLite.Api.DTOs.Discussions
{
    public class PondDiscussionStatusResponse
    {
        public Guid DiscussionPromptId { get; set; }

        public Guid RelationshipId { get; set; }

        public string PromptText { get; set; } = string.Empty;

        public DateOnly PromptDate { get; set; }

        public int ResponseCount { get; set; }

        public int ExpectedResponseCount { get; set; } = 2;

        public bool HasCurrentUserResponded { get; set; }

        public List<DiscussionResponseResponse> Responses { get; set; }
            = new List<DiscussionResponseResponse>();
    }
}