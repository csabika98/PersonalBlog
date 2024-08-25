using Microsoft.AspNetCore.Identity;

namespace PersonalBlogCsabaSallai.Models
{
    public class ApplicationRole : IdentityRole
    {
        // You don't need to override the Id property unless you want to change its behavior.
        // The base class already defines it, and it works with MongoDB.

        // If you do not require custom BSON elements, the base IdentityRole properties will work fine without explicit BSON annotations.
    }
}


