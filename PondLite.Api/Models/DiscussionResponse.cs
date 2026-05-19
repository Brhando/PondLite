using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PondLite.Api.Models
{
    public class DiscussionResponse
    {
        [Key]
        public Guid DiscussionResponseId { get; set; } = Guid.NewGuid();

        public Guid DiscussionPromptId { get; set; }

        [ForeignKey(nameof(DiscussionPromptId))]
        public DiscussionPrompt? DiscussionPrompt { get; set; }

        public Guid UserAccountId { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public UserAccount? UserAccount { get; set; }

        public string ResponseText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}