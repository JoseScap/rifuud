using Api.Services.Interfaces;

namespace Api.Services;

/// <summary>
/// Scoped service for accessing the current request's subdomain
/// </summary>
public class SubdomainService : ISubdomainService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubdomainService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Subdomain => ExtractSubdomain(Host);

    public string Host => _httpContextAccessor.HttpContext?.Request.Host.ToString() ?? string.Empty;

    /// <summary>
    /// Extract subdomain from host
    /// </summary>
    /// <param name="host">The host string (e.g., "elsultan.rifuud.com", "localhost:5000")</param>
    /// <returns>Extracted subdomain or "localhost" as default</returns>
    private static string ExtractSubdomain(string host)
    {
        if (string.IsNullOrEmpty(host))
            return "localhost";

        // Remove port if present
        var hostWithoutPort = host.Split(':')[0];
        
        // If it's localhost or IP address, return "localhost"
        if (hostWithoutPort == "localhost" || 
            hostWithoutPort.StartsWith("127.") || 
            hostWithoutPort.StartsWith("192.168.") ||
            hostWithoutPort.StartsWith("10.") ||
            System.Net.IPAddress.TryParse(hostWithoutPort, out _))
        {
            return "localhost";
        }

        // Split by dots and check if it has subdomain
        var parts = hostWithoutPort.Split('.');
        
        // If it has at least 3 parts and ends with "rifuud.com", extract subdomain
        if (parts.Length >= 3 && 
            parts[parts.Length - 2] == "rifuud" && 
            parts[parts.Length - 1] == "com")
        {
            return parts[0];
        }

        // Default to "localhost" for other cases
        return "localhost";
    }
}
