using Api.Dtos.Responses;
using Api.Models;

namespace Api.Extensions;

public static class RestaurantMapperExtensions
{   
    public static CreateRestaurantResponse ToCreateRestaurantResponse(this Restaurant restaurant) =>
        new (restaurant.Id,
            restaurant.Name,
            restaurant.Subdomain,
            restaurant.IsActive,
            restaurant.CreatedAt,
            restaurant.UpdatedAt);

    public static ListOneRestaurantResponse ToListOneRestaurantResponse(this Restaurant restaurant) =>
        new (restaurant.Id,
            restaurant.Name,
            restaurant.Subdomain,
            restaurant.IsActive,
            restaurant.CreatedAt,
            restaurant.UpdatedAt);
        
    public static ListManyRestaurantsResponse ToListManyRestaurantsResponse(this List<Restaurant> restaurants) =>
        new (restaurants.Select(r => r.ToListOneRestaurantResponse()).ToList());
}