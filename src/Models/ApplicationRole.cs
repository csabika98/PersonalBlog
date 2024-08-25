using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Identity;

namespace PersonalBlogCsabaSallai.Models
{
    public class ApplicationRole : IdentityRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public override string Id { get; set; } = ObjectId.GenerateNewId().ToString(); // Initialize with a new ObjectId

        [BsonElement("Name")]
        public override string? Name { get; set; }

        [BsonElement("NormalizedName")]
        public override string? NormalizedName { get; set; }

        [BsonElement("ConcurrencyStamp")]
        public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}

