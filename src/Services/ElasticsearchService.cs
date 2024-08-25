using Nest;
using PersonalBlogCsabaSallai.Models;

namespace PersonalBlogCsabaSallai.Services
{
    public class ElasticsearchService
    {
        private readonly IElasticClient _client;

        public ElasticsearchService(IElasticClient client)
        {
            _client = client;
        }
        public async Task CreateIndexAsync()
        {
            try
            {
                var createIndexResponse = await _client.Indices.CreateAsync("posts", c => c
                    .Map<Post>(m => m
                        .Properties(p => p
                            .Text(t => t
                                .Name(n => n.Title)
                                .Fields(f => f
                                    .Keyword(k => k.Name("keyword"))  // Add keyword subfield
                                )
                            )
                            .Text(t => t
                                .Name(n => n.Content)
                                .Fields(f => f
                                    .Keyword(k => k.Name("keyword"))  // Add keyword subfield
                                )
                            )
                            .Date(d => d
                                .Name(n => n.CreatedAt)
                            )
                            .Date(d => d
                                .Name(n => n.UpdatedAt)
                            )
                        )
                    )
                );

                if (!createIndexResponse.IsValid && createIndexResponse.ServerError.Error.Type != "resource_already_exists_exception")
                {
                    throw new Exception($"Failed to create index: {createIndexResponse.DebugInformation}");
                }
                else if (createIndexResponse.ServerError?.Error.Type == "resource_already_exists_exception")
                {
                    Console.WriteLine("Index already exists, skipping creation.");
                }
            }
            catch (Elasticsearch.Net.ElasticsearchClientException ex)
            {
                if (ex.Response.OriginalException?.Message.Contains("resource_already_exists_exception") == true)
                {
                    Console.WriteLine("Index already exists, skipping creation.");
                }
                else
                {
                    throw new Exception($"Failed to create index: {ex.Message}");
                }
            }
        }
    
        public async Task IndexPostAsync(Post post)
        {
            var response = await _client.IndexDocumentAsync(post);

            if (!response.IsValid)
            {
                throw new Exception($"Failed to index document: {response.OriginalException.Message}");
            }
        }

                public async Task<List<Post>> SearchPostsAsync(string query)
        {
            var response = await _client.SearchAsync<Post>(s => s
                .Index("posts")
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            bs => bs.Wildcard(c => c
                                .Field(p => p.Title.Suffix("keyword"))  // Use keyword suffix to match exact term
                                .Value($"*{query}*")
                            ),
                            bs => bs.Wildcard(c => c
                                .Field(p => p.Content.Suffix("keyword"))  // Use keyword suffix to match exact term
                                .Value($"*{query}*")
                            )
                        )
                    )
                )
            );

            if (!response.IsValid)
            {
                var errorMessage = response.OriginalException != null
                    ? response.OriginalException.Message
                    : "An unknown error occurred during the search.";

                throw new Exception($"Failed to search documents: {errorMessage}");
            }

            return response.Documents.ToList();
        }

        public async Task DeletePostFromIndexAsync(string postId)
        {
            var response = await _client.DeleteAsync<Post>(postId, d => d.Index("posts"));

            if (!response.IsValid)
            {
                throw new Exception($"Failed to delete document: {response.OriginalException.Message}");
            }
        }
    }
}

