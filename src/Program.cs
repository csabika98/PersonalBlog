using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Nest;


var builder = WebApplication.CreateBuilder(args);

// Elasticsearch setup
var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
    .DefaultIndex("posts");

var client = new ElasticClient(settings);

// Register Elasticsearch services
builder.Services.AddSingleton<IElasticClient>(client);
builder.Services.AddScoped<PersonalBlogCsabaSallai.Services.ElasticsearchService>();


// MongoDB settings
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
var mongoDatabase = mongoClient.GetDatabase(builder.Configuration["MongoDbSettings:DatabaseName"]);

// Register services in the DI container
builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton(mongoDatabase);
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.MongoDbContext>();

// Register application services
// Add services to the container.
// Change the registration of PostService to Scoped
builder.Services.AddScoped<PersonalBlogCsabaSallai.Services.PostService>();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.ViewLocatorService>();


// Configure Identity to use MongoDB custom stores
builder.Services.AddIdentity<PersonalBlogCsabaSallai.Models.ApplicationUser, PersonalBlogCsabaSallai.Models.ApplicationRole>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserStore<PersonalBlogCsabaSallai.Models.ApplicationUser>, PersonalBlogCsabaSallai.Stores.UserStore>();
builder.Services.AddScoped<IRoleStore<PersonalBlogCsabaSallai.Models.ApplicationRole>, PersonalBlogCsabaSallai.Stores.RoleStore>();

// Add MVC services
builder.Services.AddControllersWithViews();

try
{
    var app = builder.Build();
    // Ensure the Elasticsearch index is created
    using (var scope = app.Services.CreateScope())
    {
        var elasticsearchService = scope.ServiceProvider.GetRequiredService<PersonalBlogCsabaSallai.Services.ElasticsearchService>();
        await elasticsearchService.CreateIndexAsync();
    }
    using (var scope = app.Services.CreateScope())
    {
        var postService = scope.ServiceProvider.GetRequiredService<PersonalBlogCsabaSallai.Services.PostService>();
        await postService.ReindexAllPostsAsync();
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    throw;
}
