using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace PersonalBlogCsabaSallai.Models
{
    public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("Title")]
    public required string Title { get; set; }

    [BsonElement("Content")]
    public required string Content { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
}
