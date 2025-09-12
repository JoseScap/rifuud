using Api.Dtos.Requests;
using Api.Dtos.Responses;

namespace Api.Services.Interfaces;

public interface IRestaurantService
{
    Task<CreateRestaurantResponse> CreateRestaurantAsync(CreateRestaurantRequest request);
    Task<ListOneRestaurantResponse> GetRestaurantByIdAsync(Guid id);
    Task<ListManyRestaurantsResponse> GetManyRestaurantsAsync();
    Task<ListOneRestaurantResponse> ToggleRestaurantStatusAsync(Guid id, bool isActive);
}
