using System.ComponentModel.DataAnnotations;

namespace PondLite.Api.Models
{
    public class Relationship
    {
        [Key]
        public Guid RelationshipId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "Our Pond";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RelationshipMember> Members { get; set; }
            = new List<RelationshipMember>();
    }
}