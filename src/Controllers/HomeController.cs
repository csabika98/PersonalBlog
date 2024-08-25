using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogCsabaSallai.Services;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PostService _postService;

    public HomeController(ILogger<HomeController> logger, PostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        // Fetch the latest posts (you can adjust the number of posts to retrieve)
        var latestPosts = await _postService.GetLatestPostsAsync(5); // Fetch the latest 5 posts
        return View(latestPosts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        ViewData["RequestId"] = requestId;
        ViewData["ShowRequestId"] = !string.IsNullOrEmpty(requestId);
        return View();
    }
}


