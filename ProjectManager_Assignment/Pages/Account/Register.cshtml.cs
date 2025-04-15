using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectManager_Assignment.Models;

namespace ProjectManager_Assignment.Pages.Account;

[RequireHttps]
public class RegisterModel(
    UserManager<UserEntity> userManager) : PageModel
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    [BindProperty]
    public RegisterFormModel RegisterForm { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ChatGPT help split full name into first name and last name
            var nameParts = RegisterForm.FullName.Split(' ');
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

            var existingUser = await _userManager.FindByEmailAsync(RegisterForm.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("RegisterForm.Email", "This email is already registered");
                return Page();
            }

            var user = new UserEntity
            {
                UserName = RegisterForm.Email,
                Email = RegisterForm.Email,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, RegisterForm.Password);

            if (result.Succeeded)
            {
                return RedirectToPage("/Account/Login");
            }

            // ChatGPT recommended adding identity errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Failed to register user: {ex.Message}");
            return Page();
        }
    }
}