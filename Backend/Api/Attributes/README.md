# Attributes

This folder contains custom validation attributes and other custom attributes used throughout the application.

## Purpose
- Custom validation logic that can be reused across different DTOs and models
- Centralized validation rules that are easy to maintain and test

## Best Practices
- Keep validation logic simple and focused on a single responsibility
- Use descriptive names that clearly indicate what the attribute validates
- Include clear error messages that help users understand validation failures
- Make attributes reusable across different contexts
- Follow the `[AttributeName]` naming convention

## Example
```csharp
[PasswordValidation]
public string Password { get; set; }
```
