using MongoDB.Driver;
using PersonalBlogCsabaSallai.Models;

namespace PersonalBlogCsabaSallai.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly ElasticsearchService _elasticsearchService;

        public PostService(MongoDbContext context, ElasticsearchService ElasticsearchService)
        {
            _posts = context.Posts;
            _elasticsearchService = ElasticsearchService;
        }
        public async Task ReindexAllPostsAsync()
        {
            var posts = await _posts.Find(post => true).ToListAsync(); // Fetch all posts from MongoDB

            foreach (var post in posts)
            {
                await _elasticsearchService.IndexPostAsync(post); // Index each post in Elasticsearch
            }
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
            await _elasticsearchService.IndexPostAsync(post); // Index the post in Elasticsearch
        }

        public async Task UpdatePostAsync(string id, Post postIn)
        {
            await _posts.ReplaceOneAsync(post => post.Id == id, postIn);
            await _elasticsearchService.IndexPostAsync(postIn); // Update the post in Elasticsearch
        }

        public async Task DeletePostAsync(string id)
        {
            await _posts.DeleteOneAsync(post => post.Id == id);
            await _elasticsearchService.DeletePostFromIndexAsync(id); // Remove the post from Elasticsearch index
        }

        // Method to get the latest posts, sorted by the CreatedAt field
        public async Task<List<Post>> GetLatestPostsAsync(int count = 5)
        {
            return await _posts.Find(post => true)
                               .SortByDescending(post => post.CreatedAt)
                               .Limit(count)
                               .ToListAsync();
        }

        // Method to search posts using Elasticsearch
        public async Task<List<Post>> SearchPostsAsync(string query)
        {
            return await _elasticsearchService.SearchPostsAsync(query);
        }
    }
}


