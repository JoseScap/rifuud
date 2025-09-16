using Api.Dtos.Responses;
using Api.Models;
using System.Security.Claims;

namespace Api.Services.Interfaces;

public interface IAuthService
{
    bool ValidateAdminUserPassword(string password);
    string HashAdminUserPassword(string password);
    bool VerifyAdminUserPassword(string password, string hashedPassword);
    string GenerateAdminUserJwtToken(AdminUser user);
    Task<LoginAdminUserResponse> LoginAdminUser(string username, string password);
    bool ValidateAdminUserJwtToken(string token);
    AdminProfileResponse GetAdminUserProfile(ClaimsPrincipal user);

    bool ValidateRestaurantUserPassword(string password);
    string HashRestaurantUserPassword(string password);
    bool VerifyRestaurantUserPassword(string password, string hashedPassword);
    string GenerateRestaurantUserJwtToken(RestaurantUser user);
    Task<LoginRestaurantUserResponse> LoginRestaurantUser(string username, string password);
    bool ValidateRestaurantUserJwtToken(string token);
    RestaurantProfileResponse GetRestaurantUserProfile(ClaimsPrincipal user);
}