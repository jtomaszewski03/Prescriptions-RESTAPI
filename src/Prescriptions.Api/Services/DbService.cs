using Prescriptions.Api.Data;
using Prescriptions.Api.DTOs;
using Prescriptions.Api.Exceptions;
using Prescriptions.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Prescriptions.Api.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto prescriptionDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (prescriptionDto.DueDate < prescriptionDto.Date)
            {
                throw new InvalidDataException("The due date cannot be earlier than Date.");
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionDto.Patient.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    Birthdate = prescriptionDto.Patient.Birthdate,
                };
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
            }

            if (prescriptionDto.Medicaments.Count > 10)
            {
                throw new InvalidDataException("The number of medicaments cannot be greater than 10.");
            }

            var medicamentIds = prescriptionDto.Medicaments.Select(m => m.IdMedicament).ToList();
            foreach (var medicamentId in medicamentIds)
            {
                var medicament = await _context.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medicamentId);
                if (medicament == null)
                {
                    throw new NotFoundException("The medicament cannot be found.");
                }
            }

            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                PatientId = patient.IdPatient,
                IdDoctor = prescriptionDto.IdDoctor,
                PrescriptionsMedicaments = prescriptionDto.Medicaments.Select(e => new PrescriptionMedicament
                {
                    IdMedicament = e.IdMedicament,
                    Dose = e.Dose,
                    Details = e.Description
                }).ToList(),
            };

            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return prescription;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<GetPatientDetailsDto> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionsMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
        {
            throw new NotFoundException("Patient not found");
        }

        return new GetPatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDetailsDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionsMedicaments.Select(pm => new MedicamentDetailsDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Dose = pm.Dose,
                        Details = pm.Details
                    }).ToList()
                }).ToList()
        };
    }

    public async Task DeletePatientAsync(int idPatient)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
        {
            throw new NotFoundException("Patient not found");
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }
}
