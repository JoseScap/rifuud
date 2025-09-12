using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Api.Attributes;

/// <summary>
/// Attribute to require AdminUser authentication for endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireAdminUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Authentication required" });
            return;
        }

        // Check if the user has the admin role
        var roleClaim = user.FindFirst(ClaimTypes.Role) ?? user.FindFirst("role");
        if (roleClaim == null || !Enum.TryParse<AdminUserRole>(roleClaim.Value, out var role))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Admin role required" });
            return;
        }

        // Additional validation can be added here if needed
        // For example, checking if the user still exists in the database
    }
}
