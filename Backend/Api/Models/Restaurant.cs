using System.Text.Json;

namespace Api.Models;

public class Restaurant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public JsonDocument Settings { get; set; } = JsonDocument.Parse("{}");
    public bool IsActive { get; set; } = true;
    public virtual ICollection<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public Restaurant() { }

    public Restaurant(string name, string subdomain, JsonDocument settings, bool isActive)
    {
        Name = name;
        Subdomain = subdomain;
        Settings = settings;
        IsActive = isActive;
    }
}