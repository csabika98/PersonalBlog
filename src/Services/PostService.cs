using MongoDB.Driver;
using PersonalBlogCsabaSallai.Models;

namespace PersonalBlogCsabaSallai.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(MongoDbContext context)
        {
            _posts = context.Posts;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _posts.Find(post => true).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(string id)
        {
            return await _posts.Find(post => post.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            await _posts.InsertOneAsync(post);
        }

        public async Task UpdatePostAsync(string id, Post postIn)
        {
            await _posts.ReplaceOneAsync(post => post.Id == id, postIn);
        }

        public async Task DeletePostAsync(string id)
        {
            await _posts.DeleteOneAsync(post => post.Id == id);
        }

        // Method to get the latest posts, sorted by the CreatedAt field
        public async Task<List<Post>> GetLatestPostsAsync(int count = 5)
        {
            return await _posts.Find(post => true)
                               .SortByDescending(post => post.CreatedAt)
                               .Limit(count)
                               .ToListAsync();
        }
    }
}

