using System.ComponentModel.DataAnnotations;

namespace Prescriptions.Api.DTOs;

public class PrescriptionMedicamentDto
{
    [Range(1, int.MaxValue)] public int IdMedicament { get; set; }
    [Range(1, int.MaxValue)] public int? Dose { get; set; }
    [Required] [MaxLength(100)] public string Description { get; set; } = string.Empty;
}