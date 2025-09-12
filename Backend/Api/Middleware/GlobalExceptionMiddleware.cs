using Api.Errors;
using System.Net;
using System.Text.Json;

namespace Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "An error occurred",
            apiCode = "INTERNAL_SERVER_ERROR",
            friendlyMessage = "Something went wrong. Please try again later.",
            statusCode = (int)HttpStatusCode.InternalServerError,
            exceptionId = Guid.NewGuid()
        };

        if (exception is HttpError httpError)
        {
            response = new
            {
                message = httpError.Message,
                apiCode = httpError.ApiCode,
                friendlyMessage = httpError.FriendlyMessage,
                statusCode = (int)httpError.StatusCode,
                exceptionId = httpError.ExceptionId
            };
        }

        context.Response.StatusCode = response.statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
