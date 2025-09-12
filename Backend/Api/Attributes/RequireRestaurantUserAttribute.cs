using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Api.Attributes;

/// <summary>
/// Attribute to require RestaurantUser authentication for endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireRestaurantUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Authentication required" });
            return;
        }

        // Check if the user is a RestaurantUser
        var userTypeClaim = user.FindFirst("userType");
        if (userTypeClaim == null || userTypeClaim.Value != "RestaurantUser")
        {
            context.Result = new UnauthorizedObjectResult(new { message = "RestaurantUser authentication required" });
            return;
        }

        // Check if the user has a role
        var roleClaim = user.FindFirst(ClaimTypes.Role) ?? user.FindFirst("role");
        if (roleClaim == null)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Valid role required" });
            return;
        }

        // Additional validation can be added here if needed
        // For example, checking if the user still exists in the database
        // or if the restaurant is still active
    }
}
