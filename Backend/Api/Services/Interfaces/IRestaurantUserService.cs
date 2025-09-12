using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Models;

namespace Api.Services.Interfaces;

public interface IRestaurantUserService
{
    Task<CreateRestaurantUserResponse> CreateRestaurantUserAsync(CreateRestaurantUserRequest request);
    Task<ListOneRestaurantUserResponse> GetRestaurantUserByIdAsync(Guid id);
    Task<ListManyRestaurantUsersResponse> GetManyRestaurantUsersAsync();
}