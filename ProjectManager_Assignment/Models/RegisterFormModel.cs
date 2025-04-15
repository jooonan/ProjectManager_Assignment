using System.ComponentModel.DataAnnotations;

namespace ProjectManager_Assignment.Models;

public class RegisterFormModel
{
    [Display(Name = "Full Name", Prompt = "Your full name")]
    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Email Address is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[^\s]+@[^\s]+\.[^\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Terms and Conditions")]
    [Required(ErrorMessage = "You must accept the Terms and Conditions")]
    public bool AcceptTerms { get; set; }
}
