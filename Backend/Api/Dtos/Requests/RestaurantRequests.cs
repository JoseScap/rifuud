using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Requests;

public class CreateRestaurantRequest
{
    [Required(ErrorMessage = "Restaurant name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Restaurant name must be between 1 and 255 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Subdomain is required")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Subdomain must be between 3 and 255 characters")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Subdomain can only contain lowercase letters, numbers, and hyphens")]
    public string Subdomain { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
