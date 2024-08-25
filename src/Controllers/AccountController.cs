using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogCsabaSallai.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlogCsabaSallai.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
                if (existingUserByUsername != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                }

                // Check if the email is already taken
                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError("Email", "This email is already taken.");
                }

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Log the errors for debugging purposes
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("ModelState is not valid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        ApplicationUser? user = null;

        if (model.UsernameOrEmail.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            Console.WriteLine($"Looking for user by email: {model.UsernameOrEmail}");
        }
        else
        {
            user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            Console.WriteLine($"Looking for user by username: {model.UsernameOrEmail}");
        }

        if (user != null)
        {
            Console.WriteLine($"User found: {user.UserName}");
            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                Console.WriteLine("Login successful.");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Console.WriteLine("Login failed: Invalid password.");
                ModelState.AddModelError(string.Empty, "Login failed: Invalid password.");
            }
        }
        else
        {
            Console.WriteLine("Login failed: User not found.");
            ModelState.AddModelError(string.Empty, "Login failed: User not found.");
        }
    }
    else
    {
        Console.WriteLine("ModelState is not valid");
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine($"Validation error: {error.ErrorMessage}");
        }
    }

    // If we got this far, something failed, redisplay form
    return View(model);
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout()
{
    await _signInManager.SignOutAsync();
    Console.WriteLine("User logged out.");
    return RedirectToAction("Index", "Home");
}
}
}

