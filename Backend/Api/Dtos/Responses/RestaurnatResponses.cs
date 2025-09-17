using Api.Models;

namespace Api.Dtos.Responses;

public class BaseRestaurantResponse : BaseDtoResponse
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public BaseRestaurantResponse() { }

    public BaseRestaurantResponse(
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

public class CreateRestaurantResponse
{
    public BaseRestaurantResponse Restaurant { get; set; } = new BaseRestaurantResponse();

    public CreateRestaurantResponse() { }

    public CreateRestaurantResponse(BaseRestaurantResponse restaurant)
    {
        Restaurant = restaurant;
    }
}

public class ListOneRestaurantResponse : BaseDtoResponse   
{
    public BaseRestaurantResponse Restaurant { get; set; } = new BaseRestaurantResponse();

    public ListOneRestaurantResponse() { }

    public ListOneRestaurantResponse(BaseRestaurantResponse restaurant)
    {
        Restaurant = restaurant;
    }
}

public class ListManyRestaurantsResponse
{
    public List<BaseRestaurantResponse> Restaurants { get; set; } = new List<BaseRestaurantResponse>();

    public ListManyRestaurantsResponse() { }

    public ListManyRestaurantsResponse(List<BaseRestaurantResponse> restaurants)
    {
        Restaurants = restaurants;
    }
}