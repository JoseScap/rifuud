using System.ComponentModel.DataAnnotations;
using Api.Attributes;

namespace Api.Dtos.Requests;

public class LoginAdminUserRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [PasswordValidation]
    public string Password { get; set; } = string.Empty;
}

public class LoginRestaurantUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
