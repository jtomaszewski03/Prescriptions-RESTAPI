using System.ComponentModel.DataAnnotations;

namespace Prescriptions.Api.Models;

public class User
{
    [Key] public int IdUser { get; set; }

    [Required] [MaxLength(200)] public string Email { get; set; }

    [Required] [MaxLength(500)] public string PasswordHash { get; set; }

    [Required] public UserRole Role { get; set; } = UserRole.User;

    public int? IdDoctor { get; set; }

    public Doctor? Doctor { get; set; }
}