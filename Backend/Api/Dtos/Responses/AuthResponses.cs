namespace Api.Dtos.Responses;

public class LoginAdminUserResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public LoginAdminUserResponse() { }

    public LoginAdminUserResponse(string accessToken)
    {
        AccessToken = accessToken;
    }
}

public class LoginRestaurantUserResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public LoginRestaurantUserResponse() { }

    public LoginRestaurantUserResponse(string accessToken)
    {
        AccessToken = accessToken;
    }
}

public class AdminProfileResponse
{
    public string Message { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }

    public AdminProfileResponse() { }

    public AdminProfileResponse(string message, string userId, string username, string role, bool isAuthenticated)
    {
        Message = message;
        UserId = userId;
        Username = username;
        Role = role;
        IsAuthenticated = isAuthenticated;
    }
}

public class RestaurantProfileResponse
{
    public string Message { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public string RestaurantSubdomain { get; set; } = string.Empty;

    public RestaurantProfileResponse() { }

    public RestaurantProfileResponse(
        string message, string userId, string username,
        string role, bool isAuthenticated, string restaurantSubdomain)
    {
        Message = message;
        UserId = userId;
        Username = username;
        Role = role;
        IsAuthenticated = isAuthenticated;
        RestaurantSubdomain = restaurantSubdomain;
    }
}