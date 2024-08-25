using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.ViewLocatorService>();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.MongoDbContext>();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.PostService>();

// MongoDB settings
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
var mongoDatabase = mongoClient.GetDatabase(builder.Configuration["MongoDbSettings:DatabaseName"]);

// Register services in the DI container
builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton(mongoDatabase);

// Configure Identity to use MongoDB custom stores
builder.Services.AddIdentity<PersonalBlogCsabaSallai.Models.ApplicationUser, PersonalBlogCsabaSallai.Models.ApplicationRole>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserStore<PersonalBlogCsabaSallai.Models.ApplicationUser>, PersonalBlogCsabaSallai.Stores.UserStore>();
builder.Services.AddScoped<IRoleStore<PersonalBlogCsabaSallai.Models.ApplicationRole>, PersonalBlogCsabaSallai.Stores.RoleStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Use the Error action in HomeController for error handling
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Add this line to enable authentication
app.UseAuthorization();

// Configure MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

