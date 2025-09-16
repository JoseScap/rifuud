namespace Api.Models;

public class BaseRestaurantEntity : BaseEntity
{
    public string RestaurantSubdomain { get; set; } = string.Empty;
    public virtual Restaurant? Restaurant { get; set; }
}