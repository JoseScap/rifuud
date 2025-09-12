namespace Api.Errors;

/// <summary>
/// Constants for API error codes to ensure consistency across the application
/// </summary>
public static class ErrorCodes
{
    // Authentication errors
    public const string AUTH_INVALID_CREDENTIALS = "AUTH_INVALID_CREDENTIALS";
    public const string AUTH_USER_NOT_FOUND = "AUTH_USER_NOT_FOUND";
    public const string AUTH_ACCOUNT_DISABLED = "AUTH_ACCOUNT_DISABLED";
    public const string AUTH_INVALID_TOKEN = "AUTH_INVALID_TOKEN";
    public const string AUTH_INSUFFICIENT_PERMISSIONS = "AUTH_INSUFFICIENT_PERMISSIONS";
    public const string AUTH_PASSWORD_REQUIREMENTS = "AUTH_PASSWORD_REQUIREMENTS";
    public const string AUTH_USERNAME_EXISTS = "AUTH_USERNAME_EXISTS";

    // Restaurant errors
    public const string RESTAURANT_NOT_FOUND = "RESTAURANT_NOT_FOUND";
    public const string RESTAURANT_SUBDOMAIN_EXISTS = "RESTAURANT_SUBDOMAIN_EXISTS";
    public const string RESTAURANT_INVALID_SUBDOMAIN = "RESTAURANT_INVALID_SUBDOMAIN";
    public const string RESTAURANT_USER_NOT_FOUND = "RESTAURANT_USER_NOT_FOUND";
    public const string RESTAURANT_USERNAME_EXISTS = "RESTAURANT_USERNAME_EXISTS";
    public const string RESTAURANT_USER_NOT_BELONGS = "RESTAURANT_USER_NOT_BELONGS";
    public const string RESTAURANT_DISABLED = "RESTAURANT_DISABLED";

    // Validation errors
    public const string VALIDATION_REQUIRED_FIELD = "VALIDATION_REQUIRED_FIELD";
    public const string VALIDATION_INVALID_FORMAT = "VALIDATION_INVALID_FORMAT";
    public const string VALIDATION_INVALID_LENGTH = "VALIDATION_INVALID_LENGTH";
    public const string VALIDATION_INVALID_VALUE = "VALIDATION_INVALID_VALUE";

    // General errors
    public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
    public const string CONFIGURATION_ERROR = "CONFIGURATION_ERROR";
    public const string DATABASE_ERROR = "DATABASE_ERROR";
}
