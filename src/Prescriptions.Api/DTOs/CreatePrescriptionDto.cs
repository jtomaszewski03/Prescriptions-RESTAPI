using System.ComponentModel.DataAnnotations;

namespace Prescriptions.Api.DTOs;

public class CreatePrescriptionDto
{
    [Required] public PatientDto Patient { get; set; } = null!;

    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public List<PrescriptionMedicamentDto> Medicaments { get; set; } = [];

    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}