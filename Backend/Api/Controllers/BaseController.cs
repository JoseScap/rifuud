using Api.Errors;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Base controller that provides subdomain access to all derived controllers
/// </summary>
public abstract class BaseController : ControllerBase
{
    private readonly ISubdomainService _subdomainService;
    private readonly IConfiguration _configuration;

    protected BaseController(ISubdomainService subdomainService, IConfiguration configuration)
    {
        _subdomainService = subdomainService;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the current request's subdomain
    /// </summary>
    protected string Subdomain => _subdomainService.Subdomain;

    /// <summary>
    /// Gets the current request's host
    /// </summary>
    protected string Host => _subdomainService.Host;

    [NonAction]
    public void ValidateBackofficeSubdomain()
    {
        Console.WriteLine("Validating backoffice subdomain");
        Console.WriteLine(Subdomain);
        Console.WriteLine(Host);
        Console.WriteLine(_configuration.GetValue<bool>("Development:AllowLocalhost"));

        if (Subdomain == "localhost" && _configuration.GetValue<bool>("Development:AllowLocalhost"))
        {
            return;
        }

        if (Subdomain != "backoffice")
        {
            throw new BadRequestError(
                "Invalid subdomain",
                ErrorCodes.AUTH_INVALID_SUBDOMAIN,
                "Invalid subdomain"
            );
        }
    }

    [NonAction]
    public void ValidateRestaurantSubdomain()
    {
        Console.WriteLine("Validating restaurant subdomain");
        Console.WriteLine(Subdomain);
        Console.WriteLine(Host);
        Console.WriteLine(_configuration.GetValue<bool>("Development:AllowLocalhost"));

        if (Subdomain == "localhost" && _configuration.GetValue<bool>("Development:AllowLocalhost"))
        {
            return;
        }

        if (Subdomain != "restaurant")
        {
            throw new BadRequestError(
                "Invalid subdomain",
                ErrorCodes.AUTH_INVALID_SUBDOMAIN,
                "Invalid subdomain"
            );
        }
    }
}
