using Api.Data;
using Api.Errors;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.Services.Interfaces;

namespace Api.Services;

public class StartupService : IStartupService
{
    private readonly RifuudDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StartupService> _logger;
    private readonly IAuthService _authService;

    public StartupService(
        RifuudDbContext context, 
        IConfiguration configuration,
        ILogger<StartupService> logger,
        IAuthService authService)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _authService = authService;
    }

    public async Task InitializeRootUserAsync()
    {
        var existingRootUser = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Role == AdminUserRole.Root);

        if (existingRootUser != null)
        {
            _logger.LogInformation("Root user already exists. No new user will be created.");
            return;
        }

        var rootUsername = _configuration["RootUser:Username"];
        var rootPassword = _configuration["RootUser:Password"];

        if (string.IsNullOrEmpty(rootUsername) || string.IsNullOrEmpty(rootPassword))
        {
            throw new InternalServerError(
                "RootUser:Username and RootUser:Password must be configured in appsettings.json", 
                ErrorCodes.CONFIGURATION_ERROR, 
                "Server configuration error. Please contact support."
            );
        }

        // Validate password format
        var isPasswordValid = await _authService.ValidateAdminUserPassword(rootPassword);
        if (!isPasswordValid)
        {
            throw new InternalServerError(
                "Root user password must be alphanumeric with at least 12 characters, including uppercase, lowercase, and numbers", 
                ErrorCodes.CONFIGURATION_ERROR, 
                "Server configuration error. Please contact support."
            );
        }

        var hashedPassword = await _authService.HashAdminUserPassword(rootPassword);

        var rootUser = new AdminUser
        {
            Id = Guid.NewGuid(),
            Username = rootUsername,
            Password = hashedPassword,
            Role = AdminUserRole.Root,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.AdminUsers.Add(rootUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Root user created successfully with username: {Username}", rootUser.Username);
    }

    public Task WatchUpServerConfig()
    {
        var criticalErrors = new List<string>();
        var warnings = new List<string>();

        var criticalVariables = new Dictionary<string, string?>
        {
            { "ConnectionStrings:DefaultConnection", _configuration.GetConnectionString("DefaultConnection") },
            { "RootUser:Username", _configuration["RootUser:Username"] },
            { "RootUser:Password", _configuration["RootUser:Password"] },
            { "Authentication:Jwt:AdminUser:SecretKey", _configuration["Authentication:Jwt:AdminUser:SecretKey"] },
            { "Authentication:Jwt:RestaurantUser:SecretKey", _configuration["Authentication:Jwt:RestaurantUser:SecretKey"] }
        };

        var warningVariables = new Dictionary<string, string?>
        {
            { "ASPNETCORE_ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
            { "ASPNETCORE_URLS", Environment.GetEnvironmentVariable("ASPNETCORE_URLS") },
            { "Logging:LogLevel:Default", _configuration["Logging:LogLevel:Default"] },
            { "Cors:AllowedOrigins", string.Join(", ", _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[0]) },
            { "Authentication:Jwt:AdminUser:Issuer", _configuration["Authentication:Jwt:AdminUser:Issuer"] },
            { "Authentication:Jwt:AdminUser:Audience", _configuration["Authentication:Jwt:AdminUser:Audience"] },
            { "Authentication:Jwt:AdminUser:ExpiryHours", _configuration["Authentication:Jwt:AdminUser:ExpiryHours"] },
            { "Authentication:Jwt:RestaurantUser:Issuer", _configuration["Authentication:Jwt:RestaurantUser:Issuer"] },
            { "Authentication:Jwt:RestaurantUser:Audience", _configuration["Authentication:Jwt:RestaurantUser:Audience"] },
            { "Authentication:Jwt:RestaurantUser:ExpiryHours", _configuration["Authentication:Jwt:RestaurantUser:ExpiryHours"] }
        };

        foreach (var variable in criticalVariables)
        {
            if (string.IsNullOrEmpty(variable.Value))
            {
                criticalErrors.Add($"Critical variable missing: {variable.Key}");
            }
        }

        foreach (var variable in warningVariables)
        {
            if (string.IsNullOrEmpty(variable.Value))
            {
                warnings.Add($"Warning variable missing: {variable.Key}");
            }
        }

        foreach (var warning in warnings)
        {
            _logger.LogWarning(warning);
        }

        if (criticalErrors.Any())
        {
            var errorMessage = "Critical environment variables missing:\n" + string.Join("\n", criticalErrors);
            _logger.LogError(errorMessage);
            throw new InternalServerError(
                errorMessage, 
                ErrorCodes.CONFIGURATION_ERROR, 
                "Server configuration error. Please contact support."
            );
        }

            _logger.LogInformation("Server configuration verified successfully. {WarningCount} warnings found.", warnings.Count);
        
        return Task.CompletedTask;
    }

    public async Task StartupServerConfiguration()
    {
        _logger.LogInformation("Starting server...");
        
        await WatchUpServerConfig();
        await InitializeRootUserAsync();
        _logger.LogInformation("Servidor iniciado correctamente");
    }
}
