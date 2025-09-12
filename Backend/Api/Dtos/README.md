# DTOs (Data Transfer Objects)

This folder contains objects used for data transfer between different layers of the application.

## Purpose
- Separate internal data models from external API contracts
- Control what data is exposed to clients
- Provide validation for incoming requests
- Structure response data consistently

## Best Practices
- Use descriptive names that indicate purpose (Request/Response)
- Include proper validation attributes
- Keep DTOs simple and focused
- Use separate folders for Requests and Responses
- Avoid exposing internal implementation details
- Use nullable types when appropriate
- Include XML documentation for API documentation
- Follow consistent naming conventions

## Folder Structure
- **Requests/**: DTOs for incoming API requests
- **Responses/**: DTOs for API responses

## Example
```csharp
public class LoginRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}
```
