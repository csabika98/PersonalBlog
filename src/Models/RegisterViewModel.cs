using System.ComponentModel.DataAnnotations;

namespace PersonalBlogCsabaSallai.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The Username is required.")]
        [StringLength(50, ErrorMessage = "The Username must be between 3 and 50 characters.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The Username can only contain letters and numbers.")]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The Password is required.")]
        [StringLength(100, ErrorMessage = "The Password must be at least 6 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[^\w\d]).+$", ErrorMessage = "The Password must have at least one non-alphanumeric character.")]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "The Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}

