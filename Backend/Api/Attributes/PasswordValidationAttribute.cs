using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Api.Attributes;

public class PasswordValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string password)
            return false;

        // Check minimum length
        if (password.Length < 12)
        {
            ErrorMessage = "Password must be at least 12 characters long";
            return false;
        }

        // Check for uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            ErrorMessage = "Password must contain at least one uppercase letter";
            return false;
        }

        // Check for lowercase letter
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            ErrorMessage = "Password must contain at least one lowercase letter";
            return false;
        }

        // Check for number
        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            ErrorMessage = "Password must contain at least one number";
            return false;
        }

        // Check for alphanumeric only (no special characters)
        if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
        {
            ErrorMessage = "Password must contain only alphanumeric characters (letters and numbers)";
            return false;
        }

        return true;
    }
}
