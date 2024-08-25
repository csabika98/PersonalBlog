using Microsoft.AspNetCore.Mvc;
using PersonalBlogCsabaSallai.Models;
using PersonalBlogCsabaSallai.Services;

public class PostController : Controller
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var posts = await _postService.GetAllPostsAsync();
        return View(posts);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Post post)
    {
        if (ModelState.IsValid)
        {
            await _postService.AddPostAsync(post);
            return RedirectToAction("Index");
        }

        return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new List<Post>());
        }

        var results = await _postService.SearchPostsAsync(query);
        return View(results);
    }
}

