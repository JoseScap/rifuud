using System.Text.Json;
using Api.Dtos.Requests;

namespace Api.Models;

public class Restaurant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public JsonDocument Settings { get; set; } = JsonDocument.Parse("{}");
    public bool IsActive { get; set; } = true;
    public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();

    public Restaurant() { }

    public Restaurant(string name, string subdomain, JsonDocument settings, bool isActive)
    {
        Name = name;
        Subdomain = subdomain;
        Settings = settings;
        IsActive = isActive;
    }

    public static Restaurant FromRequest(CreateRestaurantRequest request) =>
        new (request.Name, request.Subdomain, JsonDocument.Parse("{}"), request.IsActive);
}