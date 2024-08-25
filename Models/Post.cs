using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogCsabaSallai.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        // Here im adding some basic validation to the model
        [BsonElement("Title")]
        [Required(ErrorMessage = "The Title is required.")]
        [StringLength(100, ErrorMessage = "The Title must be between 5 and 100 characters.", MinimumLength = 5)]
        public required string Title { get; set; }

        [BsonElement("Content")]
        [Required(ErrorMessage = "Content is required.")]
        [MinLength(10, ErrorMessage = "The Content must be at least 10 characters long.")]
        public required string Content { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}


