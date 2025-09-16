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

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger,
        ISubdomainService subdomainService,
        IConfiguration configuration) 
        : base(subdomainService, configuration)
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
    [ValidateBackofficeSubdomain]
    public async Task<IActionResult> Login([FromBody] LoginAdminUserRequest request)
    {
        var response = await _authService.LoginAdminUser(request.Username, request.Password);
        
        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        
        return Ok(response);
    }

    /// <summary>
    /// Get current admin user profile information
    /// </summary>
    /// <returns>Current admin user profile</returns>
    [HttpGet("Admin/Profile")]
    [Authorize(AuthenticationSchemes = "AdminUser")]
    [ProducesResponseType(typeof(AdminProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ValidateBackofficeSubdomain]
    public async Task<IActionResult> GetAdminUserProfile()
    {
        var response = await _authService.GetAdminUserProfile(HttpContext.User);
        return Ok(response);
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
    [ValidateRestaurantSubdomain]
    public async Task<IActionResult> LoginRestaurantUser([FromBody] LoginRestaurantUserRequest request)
    {
        var response = await _authService.LoginRestaurantUser(request.Username, request.Password);
        
        _logger.LogInformation("Restaurant user {Username} from {Subdomain} logged in successfully", 
            request.Username, Subdomain);
        
        return Ok(response);
    }

    /// <summary>
    /// Get current restaurant user profile information
    /// </summary>
    /// <returns>Current restaurant user profile</returns>
    [HttpGet("Restaurant/Profile")]
    [Authorize(AuthenticationSchemes = "RestaurantUser")]
    [ProducesResponseType(typeof(RestaurantProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ValidateRestaurantSubdomain]
    public async Task<IActionResult> GetRestaurantUserProfile()
    {
        var response = await _authService.GetRestaurantUserProfile(HttpContext.User);
        return Ok(response);
    }

}

