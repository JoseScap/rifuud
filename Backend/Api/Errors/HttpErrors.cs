using System.Net;

namespace Api.Errors;

/// <summary>
/// Base HTTP error class that returns status 500 by default
/// </summary>
public class HttpError : Exception
{
    /// <summary>
    /// The HTTP status code associated with this error
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    
    /// <summary>
    /// A unique code to identify the specific error type for API consumers
    /// </summary>
    public string ApiCode { get; }
    
    /// <summary>
    /// A user-friendly message intended for frontend display
    /// </summary>
    public string FriendlyMessage { get; }

    /// <summary>
    /// A unique ID for the exception to help with debugging
    /// </summary>
    public Guid ExceptionId { get; }
    
    public HttpError(string message, string apiCode, string friendlyMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base(message)
    {
        StatusCode = statusCode;
        ApiCode = apiCode;
        FriendlyMessage = friendlyMessage;
        ExceptionId = Guid.NewGuid();
    }
    
    public HttpError(string message, string apiCode, string friendlyMessage, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ApiCode = apiCode;
        FriendlyMessage = friendlyMessage;
        ExceptionId = Guid.NewGuid();
    }
}

/// <summary>
/// Bad Request error (400)
/// </summary>
public class BadRequestError : HttpError
{
    public BadRequestError(string message, string apiCode, string friendlyMessage) 
        : base(message, apiCode, friendlyMessage, HttpStatusCode.BadRequest) { }
    public BadRequestError(string message, string apiCode, string friendlyMessage, Exception innerException) 
        : base(message, apiCode, friendlyMessage, innerException, HttpStatusCode.BadRequest) { }
}

/// <summary>
/// Unauthorized error (401)
/// </summary>
public class UnauthorizedError : HttpError
{
    public UnauthorizedError(string message, string apiCode, string friendlyMessage) 
        : base(message, apiCode, friendlyMessage, HttpStatusCode.Unauthorized) { }
    public UnauthorizedError(string message, string apiCode, string friendlyMessage, Exception innerException) 
        : base(message, apiCode, friendlyMessage, innerException, HttpStatusCode.Unauthorized) { }
}

/// <summary>
/// Forbidden error (403)
/// </summary>
public class ForbiddenError : HttpError
{
    public ForbiddenError(string message, string apiCode, string friendlyMessage) 
        : base(message, apiCode, friendlyMessage, HttpStatusCode.Forbidden) { }
    public ForbiddenError(string message, string apiCode, string friendlyMessage, Exception innerException) 
        : base(message, apiCode, friendlyMessage, innerException, HttpStatusCode.Forbidden) { }
}

/// <summary>
/// Not Found error (404)
/// </summary>
public class NotFoundError : HttpError
{
    public NotFoundError(string message, string apiCode, string friendlyMessage) 
        : base(message, apiCode, friendlyMessage, HttpStatusCode.NotFound) { }
    public NotFoundError(string message, string apiCode, string friendlyMessage, Exception innerException) 
        : base(message, apiCode, friendlyMessage, innerException, HttpStatusCode.NotFound) { }
}

/// <summary>
/// Internal Server Error (500)
/// </summary>
public class InternalServerError : HttpError
{
    public InternalServerError(string message, string apiCode, string friendlyMessage) 
        : base(message, apiCode, friendlyMessage, HttpStatusCode.InternalServerError) { }
    public InternalServerError(string message, string apiCode, string friendlyMessage, Exception innerException) 
        : base(message, apiCode, friendlyMessage, innerException, HttpStatusCode.InternalServerError) { }
}