using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Prescriptions.Api.Models;

[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
[Table("Prescription_Medicament")]
public class PrescriptionMedicament
{
    [Key]
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    [Key]
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    [MaxLength(100)]
    public string Details { get; set; }

    public virtual Medicament Medicament { get; set; }
    public virtual Prescription Prescription { get; set; }
}