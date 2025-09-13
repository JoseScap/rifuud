using Api.Models;

namespace Api.Dtos.Responses;

public class CreateRestaurantResponse : BaseDtoResponse
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public CreateRestaurantResponse() { }

    public CreateRestaurantResponse(
        Guid id, string name, string subdomain,
        bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Subdomain = subdomain;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}

public class ListOneRestaurantResponse : BaseDtoResponse   
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ListOneRestaurantResponse() { }

    public ListOneRestaurantResponse(
        Guid id, string name, string subdomain,
        bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Subdomain = subdomain;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}

public class ListManyRestaurantsResponse
{
    public List<ListOneRestaurantResponse> Restaurants { get; set; } = new List<ListOneRestaurantResponse>();

    public ListManyRestaurantsResponse() { }

    public ListManyRestaurantsResponse(List<ListOneRestaurantResponse> restaurants)
    {
        Restaurants = restaurants;
    }
}