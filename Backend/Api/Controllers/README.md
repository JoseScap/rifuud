# Controllers

This folder contains all API controllers that handle HTTP requests and responses.

## Purpose
- Define API endpoints and their behavior
- Handle HTTP request/response mapping
- Implement business logic coordination
- Manage authentication and authorization

## Best Practices
- Keep controllers thin - delegate business logic to services
- Use proper HTTP status codes and response types
- Include comprehensive XML documentation for Swagger
- Validate input using data annotations
- Handle exceptions gracefully with proper error responses
- Follow RESTful naming conventions
- Use dependency injection for services
- Keep action methods focused and single-purpose

## Example Structure
```csharp
[ApiController]
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    // Controller implementation
}
```
