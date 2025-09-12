using Api.Dtos.Requests;

namespace Api.Models;

public class RestaurantUser : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public RestaurantUserRole? Role { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid RestaurantId { get; set; }
    public string RestaurantSubdomain { get; set; } = string.Empty;
    public Restaurant? Restaurant { get; set; }

    public RestaurantUser() { }

    public RestaurantUser(
        string firstName, string lastName, string phone,
        bool isActive, RestaurantUserRole? role, string username,
        string password, Guid restaurantId, string restaurantSubdomain,
        Restaurant? restaurant = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        IsActive = isActive;
        Role = role;
        Username = username;
        Password = password;
        RestaurantId = restaurantId;
        RestaurantSubdomain = restaurantSubdomain;
        Restaurant = restaurant;
    }

    public static RestaurantUser FromRequest(CreateRestaurantUserRequest request, Restaurant restaurant) =>
        new (request.FirstName, request.LastName, request.Phone,
        isActive: true, request.Role, request.Username,
        request.Password, restaurant.Id, restaurant.Subdomain);
}