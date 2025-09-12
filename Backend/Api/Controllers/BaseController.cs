using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Base controller that provides subdomain access to all derived controllers
/// </summary>
public abstract class BaseController : ControllerBase
{
    private readonly ISubdomainService _subdomainService;

    protected BaseController(ISubdomainService subdomainService)
    {
        _subdomainService = subdomainService;
    }

    /// <summary>
    /// Gets the current request's subdomain
    /// </summary>
    protected string Subdomain => _subdomainService.Subdomain;

    /// <summary>
    /// Gets the current request's host
    /// </summary>
    protected string Host => _subdomainService.Host;
}
