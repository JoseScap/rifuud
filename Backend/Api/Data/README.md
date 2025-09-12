# Data

This folder contains the database context and data access layer components.

## Purpose
- Define database schema and relationships
- Configure Entity Framework Core
- Handle database migrations
- Manage database connections and transactions

## Best Practices
- Keep DbContext focused on data access only
- Use fluent API for complex configurations
- Define proper indexes and constraints
- Use meaningful table and column names
- Implement proper foreign key relationships
- Keep sensitive configuration in appsettings
- Use connection string management
- Follow database naming conventions

## Example
```csharp
public class ExampleDbContext : DbContext
{
    public DbSet<Entity> Entities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity configurations
    }
}
```
