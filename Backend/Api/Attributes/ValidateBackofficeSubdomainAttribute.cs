using Api.Controllers;
using Api.Errors;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Attributes;

/// <summary>
/// Action filter that validates the subdomain is 'backoffice' for backoffice controllers
/// </summary>
public class ValidateBackofficeSubdomainAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.Controller as BaseController;
        if (controller == null)
        {
            throw new InvalidOperationException("ValidateBackofficeSubdomainAttribute can only be used on controllers that inherit from BaseController");
        }

        controller.ValidateBackofficeSubdomain();
        base.OnActionExecuting(context);
    }
}
