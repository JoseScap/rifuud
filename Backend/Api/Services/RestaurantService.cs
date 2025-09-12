using Api.Data;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Errors;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RifuudDbContext _context;
    private readonly ILogger<RestaurantService> _logger;

    public RestaurantService(RifuudDbContext context, ILogger<RestaurantService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateRestaurantResponse> CreateRestaurantAsync(CreateRestaurantRequest request)
    {
        // Check if subdomain already exists
        var existingRestaurant = await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Subdomain == request.Subdomain);

        if (existingRestaurant != null)
        {
            throw new BadRequestError(
                $"A restaurant with subdomain '{request.Subdomain}' already exists", 
                ErrorCodes.RESTAURANT_SUBDOMAIN_EXISTS, 
                "This subdomain is already taken. Please choose a different one."
            );
        }

        var restaurant = Restaurant.FromRequest(request);
        

        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Restaurant '{Name}' with subdomain '{Subdomain}' created successfully", 
            restaurant.Name, restaurant.Subdomain);

        return CreateRestaurantResponse.FromDomain(restaurant);
    }

    public async Task<ListOneRestaurantResponse> GetRestaurantByIdAsync(Guid id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
        {
            throw new NotFoundError(
                message: $"Restaurant with ID '{id}' not found in database",
                apiCode: ErrorCodes.RESTAURANT_NOT_FOUND,
                friendlyMessage: "The restaurant you're looking for doesn't exist"
            );
        }

        return ListOneRestaurantResponse.FromDomain(restaurant);
    }

    public async Task<ListManyRestaurantsResponse> GetManyRestaurantsAsync()
    {
        var restaurants = await _context.Restaurants.ToListAsync();
        return ListManyRestaurantsResponse.FromDomain(restaurants);
    }

    public async Task<ListOneRestaurantResponse> ToggleRestaurantStatusAsync(Guid id, bool isActive)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
        {
            throw new NotFoundError(
                message: $"Restaurant with ID '{id}' not found in database",
                apiCode: ErrorCodes.RESTAURANT_NOT_FOUND,
                friendlyMessage: "The restaurant you're looking for doesn't exist"
            );
        }

        // Update the IsActive status
        restaurant.IsActive = isActive;
        restaurant.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Restaurant '{Name}' status changed to {Status}", 
            restaurant.Name, isActive ? "Active" : "Inactive");

        return ListOneRestaurantResponse.FromDomain(restaurant);
    }
}
