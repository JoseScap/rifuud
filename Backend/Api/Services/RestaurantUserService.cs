using Api.Data;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Errors;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Api.Extensions;

namespace Api.Services;

public class RestaurantUserService : IRestaurantUserService
{
    private readonly RifuudDbContext _context;
    private readonly ILogger<RestaurantUserService> _logger;
    private readonly IAuthService _authService;

    public RestaurantUserService(
        RifuudDbContext context, 
        ILogger<RestaurantUserService> logger,
        IAuthService authService)
    {
        _context = context;
        _logger = logger;
        _authService = authService;
    }

    public async Task<CreateRestaurantUserResponse> CreateRestaurantUserAsync(CreateRestaurantUserRequest request)
    {
        // Validate that the restaurant exists
        var restaurant = await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId);

        if (restaurant == null)
        {
            throw new NotFoundError(
                $"Restaurant with ID {request.RestaurantId} not found", 
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
        var isPasswordValid = await _authService.ValidateRestaurantUserPassword(request.Password);
        if (!isPasswordValid)
        {
            throw new BadRequestError(
                "Password must be alphanumeric with at least 8 characters, including uppercase, lowercase, and numbers", 
                ErrorCodes.AUTH_PASSWORD_REQUIREMENTS, 
                "Password must be alphanumeric with at least 8 characters, including uppercase, lowercase, and numbers."
            );
        }

        // Hash password
        var hashedPassword = await _authService.HashRestaurantUserPassword(request.Password);
        request.Password = hashedPassword;

        // Create new restaurant user
        var restaurantUser = request.ToRestaurantUser(restaurant);

        _context.RestaurantUsers.Add(restaurantUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Restaurant user '{Username}' created successfully for restaurant '{Subdomain}'", 
            restaurantUser.Username, restaurantUser.RestaurantSubdomain);

        return restaurantUser.ToCreateRestaurantUserResponse();
    }

    public async Task<ListOneRestaurantUserResponse> GetRestaurantUserByIdAsync(Guid id)
    {
        var restaurantUser = await _context.RestaurantUsers
            .FirstOrDefaultAsync(ru => ru.Id == id);

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

    public async Task<ListManyRestaurantUsersResponse> GetManyRestaurantUsersAsync()
    {
        var restaurantUsers = await _context.RestaurantUsers
            .OrderBy(ru => ru.FirstName)
            .ThenBy(ru => ru.LastName)
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} restaurant users successfully", restaurantUsers.Count);

        return restaurantUsers.ToListManyRestaurantUsersResponse();
    }
    
}