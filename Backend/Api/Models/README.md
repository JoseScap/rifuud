# Models

This folder contains domain models and entity classes that represent the core business objects.

## Purpose
- Define the core business entities
- Represent database table structures
- Establish relationships between entities
- Define business rules and constraints

## Best Practices
- Keep models focused on data representation
- Use appropriate data types and constraints
- Include proper validation attributes
- Use meaningful property names
- Implement proper relationships (one-to-many, many-to-many)
- Use enums for fixed value sets
- Include audit fields (CreatedAt, UpdatedAt)
- Follow single responsibility principle
- Use nullable types when appropriate

## Example
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

## Naming Conventions
- Use PascalCase for property names
- Use descriptive names that clearly indicate purpose
- Use enums for related constant values
