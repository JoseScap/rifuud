namespace Api.Services.Interfaces;

/// <summary>
/// Service for accessing the current request's subdomain
/// </summary>
public interface ISubdomainService
{
    /// <summary>
    /// Gets the current request's subdomain
    /// </summary>
    string Subdomain { get; }
    
    /// <summary>
    /// Gets the current request's host
    /// </summary>
    string Host { get; }
    
    /// <summary>
    /// Gets the client's host from Origin/Referer headers
    /// </summary>
    string ClientHost { get; }
}
