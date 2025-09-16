using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Api.Data;
using Api.Dtos.Responses;
using Api.Errors;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class AuthService : IAuthService
{
    private readonly RifuudDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _subdomain;

    public AuthService(RifuudDbContext context, IConfiguration configuration, ISubdomainService subdomainService)
    {
        _context = context;
        _configuration = configuration;
        _subdomain = subdomainService.Subdomain;
    }

    public Task<bool> ValidateAdminUserPassword(string password)
    {
        // Check minimum length
        if (password.Length < 12)
        {
            return Task.FromResult(false);
        }

        // Check for uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            return Task.FromResult(false);
        }

        // Check for lowercase letter
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            return Task.FromResult(false);
        }

        // Check for number
        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            return Task.FromResult(false);
        }

        // Check for alphanumeric only (no special characters)
        if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
    public Task<string> HashAdminUserPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
        
        Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
        
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(combinedBytes);
        
        var saltBase64 = Convert.ToBase64String(saltBytes);
        var hashBase64 = Convert.ToBase64String(hashBytes);
        var result = $"{saltBase64}:{hashBase64}";
        
        return Task.FromResult(result);
    }

    public Task<bool> VerifyAdminUserPassword(string password, string hashedPassword)
    {
        try
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                return Task.FromResult(false);
            
            var saltBytes = Convert.FromBase64String(parts[0]);
            var storedHashBytes = Convert.FromBase64String(parts[1]);
            
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
            
            using var sha256 = SHA256.Create();
            var computedHashBytes = sha256.ComputeHash(combinedBytes);
            
            return Task.FromResult(computedHashBytes.SequenceEqual(storedHashBytes));
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<string> GenerateAdminUserJwtToken(AdminUser user)
    {
        var jwtKey = _configuration["Authentication:Jwt:AdminUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_admin_2024";
        var jwtIssuer = _configuration["Authentication:Jwt:AdminUser:Issuer"] ?? "RifuudApi";
        var jwtAudience = _configuration["Authentication:Jwt:AdminUser:Audience"] ?? "RifuudApiAdminUsers";
        var jwtExpiryHours = int.Parse(_configuration["Authentication:Jwt:AdminUser:ExpiryHours"] ?? "24");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("role", user.Role.ToString()),
            new Claim("username", user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtExpiryHours),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(tokenString);
    }

    public async Task<LoginAdminUserResponse> LoginAdminUser(string username, string password)
    {
        // Buscar el usuario por username
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new UnauthorizedError(
                "Invalid credentials", 
                ErrorCodes.AUTH_INVALID_CREDENTIALS, 
                "The username or password you entered is incorrect. Please try again."
            );
        }

        // Verificar la contraseña
        var isPasswordValid = await VerifyAdminUserPassword(password, user.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedError(
                "Invalid credentials", 
                ErrorCodes.AUTH_INVALID_CREDENTIALS, 
                "The username or password you entered is incorrect. Please try again."
            );
        }

        // Generar token JWT
        var accessToken = await GenerateAdminUserJwtToken(user);

        return new LoginAdminUserResponse
        {
            AccessToken = accessToken
        };
    }

    public Task<bool> ValidateAdminUserJwtToken(string token)
    {
        try
        {
            var jwtKey = _configuration["Authentication:Jwt:AdminUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_admin_2024";
            var jwtIssuer = _configuration["Authentication:Jwt:AdminUser:Issuer"] ?? "RifuudApi";
            var jwtAudience = _configuration["Authentication:Jwt:AdminUser:Audience"] ?? "RifuudApiAdminUsers";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<ClaimsPrincipal?> GetAdminUserFromJwtToken(string token)
    {
        try
        {
            var jwtKey = _configuration["Authentication:Jwt:AdminUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_admin_2024";
            var jwtIssuer = _configuration["Authentication:Jwt:AdminUser:Issuer"] ?? "RifuudApi";
            var jwtAudience = _configuration["Authentication:Jwt:AdminUser:Audience"] ?? "RifuudApiAdminUsers";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return Task.FromResult<ClaimsPrincipal?>(principal);
        }
        catch
        {
            return Task.FromResult<ClaimsPrincipal?>(null);
        }
    }

    // RestaurantUser Authentication Methods
    public Task<bool> ValidateRestaurantUserPassword(string password)
    {
        // Check minimum length
        if (password.Length < 8)
        {
            return Task.FromResult(false);
        }

        // Check for uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            return Task.FromResult(false);
        }

        // Check for lowercase letter
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            return Task.FromResult(false);
        }

        // Check for number
        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            return Task.FromResult(false);
        }

        // Check for alphanumeric only (no special characters)
        if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Task<string> HashRestaurantUserPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
        
        Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
        
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(combinedBytes);
        
        var saltBase64 = Convert.ToBase64String(saltBytes);
        var hashBase64 = Convert.ToBase64String(hashBytes);
        var result = $"{saltBase64}:{hashBase64}";
        
        return Task.FromResult(result);
    }

    public Task<bool> VerifyRestaurantUserPassword(string password, string hashedPassword)
    {
        try
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                return Task.FromResult(false);
            
            var saltBytes = Convert.FromBase64String(parts[0]);
            var storedHashBytes = Convert.FromBase64String(parts[1]);
            
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
            
            using var sha256 = SHA256.Create();
            var computedHashBytes = sha256.ComputeHash(combinedBytes);
            
            return Task.FromResult(computedHashBytes.SequenceEqual(storedHashBytes));
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<string> GenerateRestaurantUserJwtToken(RestaurantUser user)
    {
        var jwtKey = _configuration["Authentication:Jwt:RestaurantUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_restaurant_2024";
        var jwtIssuer = _configuration["Authentication:Jwt:RestaurantUser:Issuer"] ?? "RifuudApi";
        var jwtAudience = _configuration["Authentication:Jwt:RestaurantUser:Audience"] ?? "RifuudApiRestaurantUsers";
        var jwtExpiryHours = int.Parse(_configuration["Authentication:Jwt:RestaurantUser:ExpiryHours"] ?? "8");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role?.ToString() ?? "User"),
            new Claim("role", user.Role?.ToString() ?? "NoRole"),
            new Claim("username", user.Username),
            new Claim("restaurantId", user.Restaurant?.Id.ToString() ?? string.Empty),
            new Claim("restaurantSubdomain", user.RestaurantSubdomain),
            new Claim("userType", "RestaurantUser")
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtExpiryHours),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(tokenString);
    }

    public async Task<LoginRestaurantUserResponse> LoginRestaurantUser(string username, string password)
    {
        var user = await _context.RestaurantUsers
            .Include(ru => ru.Restaurant)
            .FirstOrDefaultAsync(u => u.Username == username && u.RestaurantSubdomain == _subdomain);

        if (user == null)
        {
            throw new UnauthorizedError(
                "Invalid credentials", 
                ErrorCodes.AUTH_INVALID_CREDENTIALS, 
                "The username or password you entered is incorrect. Please try again."
            );
        }

        // Verificar la contraseña
        var isPasswordValid = await VerifyRestaurantUserPassword(password, user.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedError(
                "Invalid credentials", 
                ErrorCodes.AUTH_INVALID_CREDENTIALS, 
                "The username or password you entered is incorrect. Please try again."
            );
        }

        // Generar token JWT
        var accessToken = await GenerateRestaurantUserJwtToken(user);

        return new LoginRestaurantUserResponse(accessToken);
    }

    public Task<bool> ValidateRestaurantUserJwtToken(string token)
    {
        try
        {
            var jwtKey = _configuration["Authentication:Jwt:RestaurantUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_restaurant_2024";
            var jwtIssuer = _configuration["Authentication:Jwt:RestaurantUser:Issuer"] ?? "RifuudApi";
            var jwtAudience = _configuration["Authentication:Jwt:RestaurantUser:Audience"] ?? "RifuudApiRestaurantUsers";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<ClaimsPrincipal?> GetRestaurantUserFromJwtToken(string token)
    {
        try
        {
            var jwtKey = _configuration["Authentication:Jwt:RestaurantUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_restaurant_2024";
            var jwtIssuer = _configuration["Authentication:Jwt:RestaurantUser:Issuer"] ?? "RifuudApi";
            var jwtAudience = _configuration["Authentication:Jwt:RestaurantUser:Audience"] ?? "RifuudApiRestaurantUsers";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return Task.FromResult<ClaimsPrincipal?>(principal);
        }
        catch
        {
            return Task.FromResult<ClaimsPrincipal?>(null);
        }
    }

    public Task<AdminProfileResponse> GetAdminUserProfile(ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        var response = new AdminProfileResponse(
            "AdminUser JWT authentication successful",
            userId,
            username,
            role,
            user.Identity?.IsAuthenticated ?? false
        );

        return Task.FromResult(response);
    }

    public Task<RestaurantProfileResponse> GetRestaurantUserProfile(ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        var restaurantId = user.FindFirst("restaurantId")?.Value ?? string.Empty;
        var restaurantSubdomain = user.FindFirst("restaurantSubdomain")?.Value ?? string.Empty;
        var userType = user.FindFirst("userType")?.Value ?? string.Empty;

        var response = new RestaurantProfileResponse(
            "RestaurantUser JWT authentication successful",
            userId,
            username,
            role,
            user.Identity?.IsAuthenticated ?? false,
            restaurantSubdomain
        );

        return Task.FromResult(response);
    }
}
