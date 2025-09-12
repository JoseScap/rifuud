using Api.Attributes;
using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

/// <summary>
/// Authentication controller for AdminUser login and JWT verification
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger, ISubdomainService subdomainService) 
        : base(subdomainService)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login an admin user
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT access token</returns>
    [HttpPost("Admin/Login")]
    [ProducesResponseType(typeof(LoginAdminUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginAdminUserRequest request)
    {
        var response = await _authService.LoginAdminUser(request.Username, request.Password);
        
        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        
        return Ok(response);
    }

    /// <summary>
    /// Test endpoint to verify AdminUser JWT authentication
    /// </summary>
    /// <returns>Current admin user information</returns>
    [HttpGet("Admin/Me")]
    [Authorize(AuthenticationSchemes = "AdminUser")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetAdminUser()
    {
        var user = HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        return Ok(new AdminMeResponse(
            "AdminUser JWT authentication successful",
            userId,
            username,
            role,
            user.Identity?.IsAuthenticated ?? false
        ));
    }

    /// <summary>
    /// Login a restaurant user
    /// </summary>
    /// <param name="request">Login credentials (username format: subdomain:username)</param>
    /// <returns>JWT access token and user information</returns>
    [HttpPost("Restaurant/Login")]
    [ProducesResponseType(typeof(LoginRestaurantUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginRestaurantUser([FromBody] LoginRestaurantUserRequest request)
    {
        var response = await _authService.LoginRestaurantUser(request.Username, request.Password);
        
        _logger.LogInformation("Restaurant user {Username} from {Subdomain} logged in successfully", 
            request.Username, Subdomain);
        
        return Ok(response);
    }

    /// <summary>
    /// Test endpoint to verify RestaurantUser JWT authentication
    /// </summary>
    /// <returns>Current restaurant user information</returns>
    [HttpGet("Restaurant/Me")]
    [Authorize(AuthenticationSchemes = "RestaurantUser")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetRestaurantUser()
    {
        var user = HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        var restaurantId = user.FindFirst("restaurantId")?.Value ?? string.Empty;
        var restaurantSubdomain = user.FindFirst("restaurantSubdomain")?.Value ?? string.Empty;
        var userType = user.FindFirst("userType")?.Value ?? string.Empty;

        return Ok(new RestaurantMeResponse(
            "RestaurantUser JWT authentication successful",
            userId,
            username,
            role,
            user.Identity?.IsAuthenticated ?? false,
            restaurantSubdomain
        ));
    }

}

