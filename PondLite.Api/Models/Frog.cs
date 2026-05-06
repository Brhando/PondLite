using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PondLite.Api.Models.Enums;

namespace PondLite.Api.Models
{
    public class Frog
    {
        [Key]
        public Guid FrogId { get; set; } = Guid.NewGuid();

        public Guid RelationshipMemberId { get; set; }

        [ForeignKey(nameof(RelationshipMemberId))]
        public RelationshipMember? RelationshipMember { get; set; }

        public string Name { get; set; } = "Little Frog";

        public FrogMood CurrentMood { get; set; } = FrogMood.None;

        public FrogActivityState ActivityState { get; set; } = FrogActivityState.Asleep;

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
