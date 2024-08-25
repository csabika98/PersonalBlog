using System.ComponentModel.DataAnnotations;

namespace PersonalBlogCsabaSallai.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The Username or Email is required.")]
        [Display(Name = "Username or Email")]
        public required string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "The Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
