using MongoDB.Driver;
using PersonalBlogCsabaSallai.Models;

namespace PersonalBlogCsabaSallai.Services
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
        }

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
    }
}
