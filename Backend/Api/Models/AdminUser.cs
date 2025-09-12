using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class AdminUser : BaseEntity
{
    public AdminUserRole Role { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
