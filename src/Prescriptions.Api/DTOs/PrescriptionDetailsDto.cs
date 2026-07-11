namespace Prescriptions.Api.DTOs;

public class PrescriptionDetailsDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDetailsDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}