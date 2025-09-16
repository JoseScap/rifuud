namespace Api.Models;

public class RestaurantUser : BaseRestaurantEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public RestaurantUserRole? Role { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public RestaurantUser() { }

    public RestaurantUser(
        string firstName, string lastName, string phone,
        bool isActive, RestaurantUserRole? role, string username,
        string password, string restaurantSubdomain,
        Restaurant? restaurant = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        IsActive = isActive;
        Role = role;
        Username = username;
        Password = password;
        RestaurantSubdomain = restaurantSubdomain;
        Restaurant = restaurant;
    }
}