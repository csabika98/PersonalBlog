using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Identity;

// Creating a new class ApplicationUser

namespace PersonalBlogCsabaSallai.Models
{
    public class ApplicationUser : IdentityUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public override string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("UserName")]
        public override string? UserName { get; set; }

        [BsonElement("NormalizedUserName")]
        public override string? NormalizedUserName { get; set; }

        [BsonElement("Email")]
        public override string? Email { get; set; }

        [BsonElement("NormalizedEmail")]
        public override string? NormalizedEmail { get; set; }

        [BsonElement("EmailConfirmed")]
        public override bool EmailConfirmed { get; set; } = false;

        [BsonElement("PasswordHash")]
        public override string? PasswordHash { get; set; }

        [BsonElement("SecurityStamp")]
        public override string? SecurityStamp { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("ConcurrencyStamp")]
        public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("PhoneNumber")]
        public override string? PhoneNumber { get; set; }

        [BsonElement("PhoneNumberConfirmed")]
        public override bool PhoneNumberConfirmed { get; set; } = false;

        [BsonElement("TwoFactorEnabled")]
        public override bool TwoFactorEnabled { get; set; } = false;

        [BsonElement("LockoutEnd")]
        public override DateTimeOffset? LockoutEnd { get; set; } = null;

        [BsonElement("LockoutEnabled")]
        public override bool LockoutEnabled { get; set; } = false;

        [BsonElement("AccessFailedCount")]
        public override int AccessFailedCount { get; set; } = 0;

        [BsonElement("Roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [BsonElement("Claims")]
        public List<ApplicationUserClaim> Claims { get; set; } = new List<ApplicationUserClaim>();
    }

    public class ApplicationUserClaim
    {
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}



