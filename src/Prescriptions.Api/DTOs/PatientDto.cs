using System.ComponentModel.DataAnnotations;

namespace Prescriptions.Api.DTOs;

public class PatientDto
{
    [Range(1, int.MaxValue)] public int IdPatient { get; set; }

    [Required] [MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required] [MaxLength(100)] public string LastName { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
}