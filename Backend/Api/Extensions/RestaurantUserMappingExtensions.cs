using Api.Dtos.Responses;
using Api.Models;

namespace Api.Extensions;

public static class RestaurantUserMappingExtensions
{
    public static CreateRestaurantUserResponse ToCreateRestaurantUserResponse(this RestaurantUser restaurantUser) =>
        new (restaurantUser.Id, restaurantUser.FirstName, restaurantUser.LastName,
        restaurantUser.Phone, restaurantUser.Role, restaurantUser.Username,
        restaurantUser.RestaurantId, restaurantUser.RestaurantSubdomain,
        restaurantUser.IsActive, restaurantUser.CreatedAt, restaurantUser.UpdatedAt);

    public static ListOneRestaurantUserResponse ToListOneRestaurantUserResponse(this RestaurantUser restaurantUser) =>
        new (restaurantUser.Id, restaurantUser.FirstName, restaurantUser.LastName,
        restaurantUser.Phone, restaurantUser.Role, restaurantUser.Username,
        restaurantUser.RestaurantId, restaurantUser.RestaurantSubdomain,
        restaurantUser.IsActive, restaurantUser.CreatedAt, restaurantUser.UpdatedAt);

    public static ListManyRestaurantUsersResponse ToListManyRestaurantUsersResponse(this List<RestaurantUser> restaurantUsers) =>
        new (restaurantUsers.Select(ru => ru.ToListOneRestaurantUserResponse()).ToList());
}