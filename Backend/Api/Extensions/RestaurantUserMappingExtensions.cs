using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Models;

namespace Api.Extensions;

public static class RestaurantUserMappingExtensions
{
    public static CreateRestaurantUserResponse ToCreateRestaurantUserResponse(this RestaurantUser restaurantUser) =>
        new (restaurantUser.Id, restaurantUser.FirstName, restaurantUser.LastName,
        restaurantUser.Phone, restaurantUser.Role, restaurantUser.Username,
        restaurantUser.Restaurant?.Id ?? Guid.Empty, restaurantUser.RestaurantSubdomain, restaurantUser.IsActive,
        restaurantUser.CreatedAt, restaurantUser.UpdatedAt);

    public static ListOneRestaurantUserResponse ToListOneRestaurantUserResponse(this RestaurantUser restaurantUser) =>
        new (restaurantUser.Id, restaurantUser.FirstName, restaurantUser.LastName,
        restaurantUser.Phone, restaurantUser.Role, restaurantUser.Username,
        restaurantUser.Restaurant?.Id ?? Guid.Empty, restaurantUser.RestaurantSubdomain, restaurantUser.IsActive,
        restaurantUser.CreatedAt, restaurantUser.UpdatedAt);

    public static ListManyRestaurantUsersResponse ToListManyRestaurantUsersResponse(this List<RestaurantUser> restaurantUsers) =>
        new (restaurantUsers.Select(ru => ru.ToListOneRestaurantUserResponse()).ToList());

    public static RestaurantUser ToRestaurantUser(this CreateRestaurantUserRequest request, Restaurant restaurant) =>
        new (request.FirstName, request.LastName, request.Phone,
        isActive: true, request.Role, request.Username,
        request.Password, restaurant.Subdomain, restaurant);
}