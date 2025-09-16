using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class RifuudDbContext : DbContext
{
    public RifuudDbContext(DbContextOptions<RifuudDbContext> options) : base(options)
    {
    }

    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantUser> RestaurantUsers { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure BaseEntity properties for all entities
        ConfigureBaseEntity<AdminUser>(modelBuilder);
        ConfigureBaseEntity<Restaurant>(modelBuilder);
        ConfigureBaseEntity<RestaurantUser>(modelBuilder);
        ConfigureBaseEntity<ProductCategory>(modelBuilder);
        
        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Role).IsRequired();
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(1024);
            entity.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
            entity.Property(u => u.UpdatedAt).IsRequired().HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(255);
            entity.Property(r => r.Subdomain).IsRequired().HasMaxLength(255);
            entity.Property(r => r.Settings).IsRequired();
            entity.Property(r => r.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
            entity.Property(r => r.UpdatedAt).IsRequired().HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();

            entity.HasIndex(r => r.Subdomain).IsUnique();
        });

        modelBuilder.Entity<RestaurantUser>(entity =>
        {
            entity.HasKey(ru => ru.Id);
            entity.Property(ru => ru.FirstName).IsRequired().HasMaxLength(255);
            entity.Property(ru => ru.LastName).IsRequired().HasMaxLength(255);
            entity.Property(ru => ru.Phone).IsRequired().HasMaxLength(255);
            entity.Property(ru => ru.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(ru => ru.Role).IsRequired();
            entity.Property(ru => ru.Username).IsRequired().HasMaxLength(255);
            entity.Property(ru => ru.Password).IsRequired().HasMaxLength(1024);
            entity.Property(ru => ru.RestaurantSubdomain).IsRequired().HasMaxLength(255);
            entity.Property(ru => ru.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
            entity.Property(ru => ru.UpdatedAt).IsRequired().HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();

            entity.HasIndex(ru => new { ru.RestaurantSubdomain, ru.Username }).IsUnique();
            
            entity.HasOne(ru => ru.Restaurant)
                  .WithMany(r => r.RestaurantUsers)
                  .HasForeignKey(ru => ru.RestaurantSubdomain)
                  .HasPrincipalKey(r => r.Subdomain)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(pc => pc.Id);
            entity.Property(pc => pc.Name).IsRequired().HasMaxLength(255);
            entity.Property(pc => pc.Description).IsRequired().HasMaxLength(255);
            entity.Property(pc => pc.Color).IsRequired().HasMaxLength(255);
            entity.Property(pc => pc.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(pc => pc.RestaurantSubdomain).IsRequired().HasMaxLength(255);
            entity.Property(pc => pc.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
            entity.Property(pc => pc.UpdatedAt).IsRequired().HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();

            entity.HasIndex(pc => new { pc.RestaurantSubdomain, pc.Name }).IsUnique();
            
            entity.HasOne(pc => pc.Restaurant)
                  .WithMany(r => r.ProductCategories)
                  .HasForeignKey(pc => pc.RestaurantSubdomain)
                  .HasPrincipalKey(r => r.Subdomain)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Configure BaseEntity properties for automatic generation
    /// </summary>
    private void ConfigureBaseEntity<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>(entity =>
        {
            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("gen_random_uuid()");
        });
    }
}
