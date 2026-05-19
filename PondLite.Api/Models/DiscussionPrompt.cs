using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PondLite.Api.Models
{
    public class DiscussionPrompt
    {
        [Key]
        public Guid DiscussionPromptId { get; set; } = Guid.NewGuid();

        public Guid RelationshipId { get; set; }

        [ForeignKey(nameof(RelationshipId))]
        public Relationship? Relationship { get; set; }

        public string PromptText { get; set; } = string.Empty;

        public DateOnly PromptDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<DiscussionResponse> Responses { get; set; }
            = new List<DiscussionResponse>();
    }
}