using System.Text.Json;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Models;

namespace Api.Extensions;

public static class RestaurantMapperExtensions
{   
    public static Restaurant ToRestaurant(this CreateRestaurantRequest request) =>
        new (request.Name, request.Subdomain, JsonDocument.Parse("{}"), request.IsActive);

    public static BaseRestaurantResponse ToBaseRestaurantResponse(this Restaurant restaurant) =>
        new (restaurant.Id,
            restaurant.Name,
            restaurant.Subdomain,
            restaurant.IsActive,
            restaurant.CreatedAt,
            restaurant.UpdatedAt);

    public static CreateRestaurantResponse ToCreateRestaurantResponse(this Restaurant restaurant) =>
        new (restaurant.ToBaseRestaurantResponse());

    public static ListOneRestaurantResponse ToListOneRestaurantResponse(this Restaurant restaurant) =>
        new (restaurant.ToBaseRestaurantResponse());
        
    public static ListManyRestaurantsResponse ToListManyRestaurantsResponse(this List<Restaurant> restaurants) =>
        new (restaurants.Select(r => r.ToBaseRestaurantResponse()).ToList());
}