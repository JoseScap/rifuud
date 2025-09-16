using Api.Data;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Errors;
using Api.Extensions;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RifuudDbContext _context;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthService _authService;

    public RestaurantService(RifuudDbContext context, ILogger<RestaurantService> logger, IAuthService authService)
    {
        _context = context;
        _logger = logger;
        _authService = authService;
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

        var restaurant = request.ToRestaurant();
        

        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Restaurant '{Name}' with subdomain '{Subdomain}' created successfully", 
            restaurant.Name, restaurant.Subdomain);

        return restaurant.ToCreateRestaurantResponse();
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

        return restaurant.ToListOneRestaurantResponse();
    }

    public async Task<ListManyRestaurantsResponse> GetManyRestaurantsAsync()
    {
        var restaurants = await _context.Restaurants.ToListAsync();
        return restaurants.ToListManyRestaurantsResponse();
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

        return restaurant.ToListOneRestaurantResponse();
    }

    public async Task<CreateRestaurantUserResponse> CreateRestaurantUserForRestaurantAsync(Guid restaurantId, CreateRestaurantUserRequest request)
    {
        // Validate that the restaurant exists
        var restaurant = await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Id == restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundError(
                $"Restaurant with ID {restaurantId} not found", 
                ErrorCodes.RESTAURANT_NOT_FOUND, 
                "The requested restaurant could not be found."
            );
        }

        // Check if username already exists for this restaurant (multi-tenant)
        var existingUser = await _context.RestaurantUsers
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.RestaurantSubdomain == restaurant.Subdomain);

        if (existingUser != null)
        {
            throw new BadRequestError(
                $"Username '{request.Username}' already exists for restaurant '{restaurant.Subdomain}'", 
                ErrorCodes.RESTAURANT_USERNAME_EXISTS, 
                "This username is already taken for this restaurant. Please choose a different one."
            );
        }

        // Validate password
        var isPasswordValid = _authService.ValidateRestaurantUserPassword(request.Password);
        if (!isPasswordValid)
        {
            throw new BadRequestError(
                "Password must be alphanumeric with at least 8 characters, including uppercase, lowercase, and numbers", 
                ErrorCodes.AUTH_PASSWORD_REQUIREMENTS, 
                "Password must be alphanumeric with at least 8 characters, including uppercase, lowercase, and numbers."
            );
        }

        // Hash password
        var hashedPassword = _authService.HashRestaurantUserPassword(request.Password);
        request.Password = hashedPassword;

        // Create new restaurant user
        var restaurantUser = request.ToRestaurantUser(restaurant);

        _context.RestaurantUsers.Add(restaurantUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Restaurant user '{Username}' created successfully for restaurant '{Subdomain}'", 
            restaurantUser.Username, restaurantUser.RestaurantSubdomain);

        return restaurantUser.ToCreateRestaurantUserResponse();
    }

    public async Task<ListOneRestaurantUserResponse> GetRestaurantUserByIdAndRestaurantIdAsync(Guid restaurantId, Guid id)
    {
        var restaurantUser = await _context.RestaurantUsers
            .Include(ru => ru.Restaurant)
            .FirstOrDefaultAsync(ru => ru.Id == id && ru.Restaurant.Id == restaurantId);

        if (restaurantUser == null)
        {
            throw new NotFoundError(
                $"Restaurant user with ID {id} not found", 
                ErrorCodes.RESTAURANT_USER_NOT_FOUND, 
                "The requested user could not be found."
            );
        }

        _logger.LogInformation("Restaurant user '{Username}' retrieved successfully", restaurantUser.Username);

        return restaurantUser.ToListOneRestaurantUserResponse();
    }

    public async Task<ListManyRestaurantUsersResponse> GetManyRestaurantUsersByRestaurantIdAsync(Guid restaurantId)
    {
        var restaurantUsers = await _context.RestaurantUsers
            .OrderBy(ru => ru.FirstName)
            .ThenBy(ru => ru.LastName)
            .Include(ru => ru.Restaurant)
            .Where(ru => ru.Restaurant.Id == restaurantId)
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} restaurant users successfully", restaurantUsers.Count);

        return restaurantUsers.ToListManyRestaurantUsersResponse();
    }
}
