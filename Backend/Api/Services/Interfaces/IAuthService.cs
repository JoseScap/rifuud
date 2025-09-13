using Api.Dtos.Responses;
using Api.Models;
using System.Security.Claims;

namespace Api.Services.Interfaces;

public interface IAuthService
{
    Task<bool> ValidateAdminUserPassword(string password);
    Task<string> HashAdminUserPassword(string password);
    Task<bool> VerifyAdminUserPassword(string password, string hashedPassword);
    Task<string> GenerateAdminUserJwtToken(AdminUser user);
    Task<LoginAdminUserResponse> LoginAdminUser(string username, string password);
    Task<bool> ValidateAdminUserJwtToken(string token);
    Task<AdminProfileResponse> GetAdminUserProfile(ClaimsPrincipal user);

    Task<bool> ValidateRestaurantUserPassword(string password);
    Task<string> HashRestaurantUserPassword(string password);
    Task<bool> VerifyRestaurantUserPassword(string password, string hashedPassword);
    Task<string> GenerateRestaurantUserJwtToken(RestaurantUser user);
    Task<LoginRestaurantUserResponse> LoginRestaurantUser(string username, string password);
    Task<bool> ValidateRestaurantUserJwtToken(string token);
    Task<RestaurantProfileResponse> GetRestaurantUserProfile(ClaimsPrincipal user);
}