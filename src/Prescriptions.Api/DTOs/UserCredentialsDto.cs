using System.ComponentModel.DataAnnotations;

namespace Prescriptions.Api.DTOs;

public class UserCredentialsDto
{
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; }
}