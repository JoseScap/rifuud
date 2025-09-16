using Api.Attributes;
using Api.Controllers;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Backoffice;

/// <summary>
/// Restaurant management controller - requires AdminUser authentication
/// </summary>
[ApiController]
[Route("Backoffice/[controller]")]
[Authorize(AuthenticationSchemes = "AdminUser")]
[ValidateBackofficeSubdomain]
public class RestaurantController : BaseController
{
    private readonly IRestaurantService _restaurantService;
    private readonly ILogger<RestaurantController> _logger;

    public RestaurantController(
        IRestaurantService restaurantService,
        ILogger<RestaurantController> logger,
        ISubdomainService subdomainService,
        IConfiguration configuration) 
        : base(subdomainService, configuration)
    {
        _restaurantService = restaurantService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new restaurant
    /// </summary>
    /// <param name="request">Restaurant creation data</param>
    /// <returns>Created restaurant information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRestaurantResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request)
    {
        var response = await _restaurantService.CreateRestaurantAsync(request);
        
        _logger.LogInformation("Restaurant '{Name}' created successfully with ID {Id}", response.Name, response.Id);
        
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }

    /// <summary>
    /// Get a restaurant by ID
    /// </summary>
    /// <param name="id">Restaurant ID</param>
    /// <returns>Restaurant information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ListOneRestaurantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _restaurantService.GetRestaurantByIdAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Get all restaurants
    /// </summary>
    /// <returns>List of all restaurants</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ListManyRestaurantsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _restaurantService.GetManyRestaurantsAsync();
        return Ok(response);
    }

    /// <summary>
    /// Activate a restaurant
    /// </summary>
    /// <param name="restaurantId">Restaurant ID</param>
    /// <returns>Activated restaurant information</returns>
    [HttpPost("{restaurantId}/activate")]
    [ProducesResponseType(typeof(ListOneRestaurantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid restaurantId)
    {
        var response = await _restaurantService.ToggleRestaurantStatusAsync(restaurantId, true);
        return Ok(response);
    }

    /// <summary>
    /// Deactivate a restaurant
    /// </summary>
    /// <param name="restaurantId">Restaurant ID</param>
    /// <returns>Deactivated restaurant information</returns>
    [HttpPost("{restaurantId}/deactivate")]
    [ProducesResponseType(typeof(ListOneRestaurantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid restaurantId)
    {
        var response = await _restaurantService.ToggleRestaurantStatusAsync(restaurantId, false);
        return Ok(response);
    }

    /// <summary>
    /// Create a new restaurant user
    /// </summary>
    /// <param name="restaurantId">Restaurant ID</param>
    /// <param name="request">Restaurant user creation data</param>
    /// <returns>Created restaurant user information</returns>
    [HttpPost("{restaurantId}/Users")]
    [ProducesResponseType(typeof(CreateRestaurantUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRestaurantUser(Guid restaurantId, [FromBody] CreateRestaurantUserRequest request)
    {
        var response = await _restaurantService.CreateRestaurantUserForRestaurantAsync(restaurantId, request);

        _logger.LogInformation("Restaurant user '{Username}' created successfully for restaurant '{RestaurantId}'", 
            response.Username, response.RestaurantId);

        return CreatedAtAction(nameof(CreateRestaurantUser), new { id = response.Id }, response);
    }

    /// <summary>
    /// Get a restaurant user by ID
    /// </summary>
    /// <param name="restaurantId">Restaurant ID</param>
    /// <param name="userId">Restaurant user ID</param>
    /// <returns>Restaurant user information</returns>
    [HttpGet("{restaurantId}/Users/{userId}")]
    [ProducesResponseType(typeof(ListOneRestaurantUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRestaurantUserById(Guid restaurantId, Guid userId)
    {
        var response = await _restaurantService.GetRestaurantUserByIdAndRestaurantIdAsync(restaurantId, userId);

        _logger.LogInformation("Restaurant user '{Username}' retrieved successfully", response.Username);

        return Ok(response);
    }

    /// <summary>
    /// Get all restaurant users
    /// </summary>
    /// <param name="restaurantId">Restaurant ID</param>
    /// <returns>List of all restaurant users</returns>
    [HttpGet("{restaurantId}/Users")]
    [ProducesResponseType(typeof(ListManyRestaurantUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetManyRestaurantUsers(Guid restaurantId)
    {
        var response = await _restaurantService.GetManyRestaurantUsersByRestaurantIdAsync(restaurantId);

        _logger.LogInformation("Retrieved {Count} restaurant users successfully", response.RestaurantUsers.Count);

        return Ok(response);
    }
}