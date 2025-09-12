using Api.Models;

namespace Api.Dtos.Responses;

public class CreateRestaurantUserResponse : BaseDtoResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RestaurantUserRole? Role { get; set; }
    public string Username { get; set; } = string.Empty;
    public Guid RestaurantId { get; set; }
    public string RestaurantSubdomain { get; set; } = string.Empty;

    public CreateRestaurantUserResponse() { }

    public CreateRestaurantUserResponse(
        Guid id, string firstName, string lastName,
        string phone, RestaurantUserRole? role, string username,
        Guid restaurantId, string restaurantSubdomain, bool isActive,
        DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Role = role;
        Username = username;
        RestaurantId = restaurantId;
        RestaurantSubdomain = restaurantSubdomain;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static CreateRestaurantUserResponse FromDomain(RestaurantUser restaurantUser) =>
        new (restaurantUser.Id, restaurantUser.FirstName, restaurantUser.LastName,
        restaurantUser.Phone, restaurantUser.Role, restaurantUser.Username,
        restaurantUser.RestaurantId, restaurantUser.RestaurantSubdomain,
        restaurantUser.IsActive, restaurantUser.CreatedAt, restaurantUser.UpdatedAt);
}

public class ListOneRestaurantUserResponse : BaseDtoResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RestaurantUserRole? Role { get; set; }
    public string Username { get; set; } = string.Empty;
    public Guid RestaurantId { get; set; }
    public string RestaurantSubdomain { get; set; } = string.Empty;

    public ListOneRestaurantUserResponse() { }

    public ListOneRestaurantUserResponse(
        Guid id, string firstName, string lastName,
        string phone, RestaurantUserRole? role, string username,
        Guid restaurantId, string restaurantSubdomain, bool isActive,
        DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Role = role;
        Username = username;
        RestaurantId = restaurantId;
        RestaurantSubdomain = restaurantSubdomain;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ListOneRestaurantUserResponse FromDomain(RestaurantUser restaurantUser) =>
        new (restaurantUser.Id, restaurantUser.FirstName, restaurantUser.LastName,
        restaurantUser.Phone, restaurantUser.Role, restaurantUser.Username,
        restaurantUser.RestaurantId, restaurantUser.RestaurantSubdomain,
        restaurantUser.IsActive, restaurantUser.CreatedAt, restaurantUser.UpdatedAt);
}

public class ListManyRestaurantUsersResponse
{
    public List<ListOneRestaurantUserResponse> RestaurantUsers { get; set; } = new List<ListOneRestaurantUserResponse>();

    public ListManyRestaurantUsersResponse() { }

    public ListManyRestaurantUsersResponse(List<ListOneRestaurantUserResponse> restaurantUsers)
    {
        RestaurantUsers = restaurantUsers;
    }

    public static ListManyRestaurantUsersResponse FromDomain(List<RestaurantUser> restaurantUsers) =>
        new (restaurantUsers.Select(ru => ListOneRestaurantUserResponse.FromDomain(ru)).ToList());
}