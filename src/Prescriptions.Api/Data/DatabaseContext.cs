using Prescriptions.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Prescriptions.Api.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionsMedicaments { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new Doctor() { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" },
            new Doctor() { IdDoctor = 2, FirstName = "Anna", LastName = "Kowalska", Email = "anna.kowalska@example.com" },
            new Doctor() { IdDoctor = 3, FirstName = "Michael", LastName = "Smith", Email = "michael.smith@example.com" },
        });
        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
        {
            new Medicament { IdMedicament = 1, Name = "Apap", Description = "Lek przeciwbólowy", Type = "Tabletki" },
            new Medicament { IdMedicament = 4, Name = "Gripex", Description = "Lek na przeziębienie", Type = "Proszek" },
            new Medicament { IdMedicament = 5, Name = "Amoxicillin", Description = "Antibiotic", Type = "Capsules" },
            new Medicament { IdMedicament = 6, Name = "Loratadine", Description = "Antihistamine", Type = "Tablets" },
            new Medicament { IdMedicament = 7, Name = "Salbutamol", Description = "Bronchodilator", Type = "Inhaler" }
        });
        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new Patient { IdPatient = 1, FirstName = "Adam", LastName = "Nowak", Birthdate = new DateTime(1998, 5, 12) },
            new Patient { IdPatient = 2, FirstName = "Maria", LastName = "Wisniewska", Birthdate = new DateTime(1984, 11, 23) },
            new Patient { IdPatient = 3, FirstName = "Piotr", LastName = "Zielinski", Birthdate = new DateTime(1975, 2, 3) }
        });
        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
        {
            new Prescription { IdPrescription = 1, Date = new DateTime(2026, 1, 10), DueDate = new DateTime(2026, 1, 24), PatientId = 1, IdDoctor = 1 },
            new Prescription { IdPrescription = 2, Date = new DateTime(2026, 2, 5), DueDate = new DateTime(2026, 2, 19), PatientId = 2, IdDoctor = 2 },
            new Prescription { IdPrescription = 3, Date = new DateTime(2026, 3, 12), DueDate = new DateTime(2026, 3, 26), PatientId = 3, IdDoctor = 3 }
        });
        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>
        {
            new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 1, Dose = 2, Details = "Take after meal" },
            new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 6, Dose = 1, Details = "Take once daily" },
            new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 5, Dose = 1, Details = "Take every 8 hours" },
            new PrescriptionMedicament { IdPrescription = 3, IdMedicament = 4, Dose = 1, Details = "Use before sleep" },
            new PrescriptionMedicament { IdPrescription = 3, IdMedicament = 7, Dose = 2, Details = "Use when needed" }
        });
        base.OnModelCreating(modelBuilder);
    }
}
