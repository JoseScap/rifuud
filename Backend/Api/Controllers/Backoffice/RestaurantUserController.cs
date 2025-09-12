using Api.Attributes;
using Api.Controllers;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Backoffice;

/// <summary>
/// Restaurant User management controller - requires AdminUser authentication
/// </summary>
[ApiController]
[Route("Backoffice/[controller]")]
[Authorize(AuthenticationSchemes = "AdminUser")]
public class RestaurantUserController : BaseController
{
    private readonly IRestaurantUserService _restaurantUserService;
    private readonly ILogger<RestaurantUserController> _logger;
    
    public RestaurantUserController(IRestaurantUserService restaurantUserService, ILogger<RestaurantUserController> logger, ISubdomainService subdomainService) 
        : base(subdomainService)
    {
        _restaurantUserService = restaurantUserService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new restaurant user
    /// </summary>
    /// <param name="request">Restaurant user creation data</param>
    /// <returns>Created restaurant user information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRestaurantUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRestaurantUser([FromBody] CreateRestaurantUserRequest request)
    {
        var response = await _restaurantUserService.CreateRestaurantUserAsync(request);

        _logger.LogInformation("Restaurant user '{Username}' created successfully for restaurant '{RestaurantId}'", 
            response.Username, response.RestaurantId);

        return CreatedAtAction(nameof(CreateRestaurantUser), new { id = response.Id }, response);
    }

    /// <summary>
    /// Get a restaurant user by ID
    /// </summary>
    /// <param name="id">Restaurant user ID</param>
    /// <returns>Restaurant user information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ListOneRestaurantUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRestaurantUserById(Guid id)
    {
        var response = await _restaurantUserService.GetRestaurantUserByIdAsync(id);

        _logger.LogInformation("Restaurant user '{Username}' retrieved successfully", response.Username);

        return Ok(response);
    }

    /// <summary>
    /// Get all restaurant users
    /// </summary>
    /// <returns>List of all restaurant users</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ListManyRestaurantUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetManyRestaurantUsers()
    {
        var response = await _restaurantUserService.GetManyRestaurantUsersAsync();

        _logger.LogInformation("Retrieved {Count} restaurant users successfully", response.RestaurantUsers.Count);

        return Ok(response);
    }
}