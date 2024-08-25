var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.ViewLocatorService>();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.MongoDbContext>();
builder.Services.AddSingleton<PersonalBlogCsabaSallai.Services.PostService>();

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

app.UseAuthorization();

// Configure MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

