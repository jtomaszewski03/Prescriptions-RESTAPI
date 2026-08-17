using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Prescriptions.Api.Data;
using Prescriptions.Api.DTOs;
using Prescriptions.Api.Exceptions;
using Prescriptions.Api.Services;

namespace Prescriptions.Api.Tests.Services;

public class DbServiceTests
{
    [Fact]
    public async Task CreatePrescriptionAsync_WhenDueDateIsEarlierThanDate_ShouldThrowInvalidDataException()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        var options = new DbContextOptionsBuilder<DbContext>().UseSqlite(connection).Options;
        await using var context = new DatabaseContext(options);
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
        var service = new DbService(context);

        var prescriptionDto = new CreatePrescriptionDto()
        {
            Patient = new PatientDto()
            {
                Birthdate = new DateTime(1990, 1, 1),
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe"
            },
            Medicaments =
            [
                new PrescriptionMedicamentDto()
                {
                    Description = "xxx",
                    Dose = 1,
                    IdMedicament = 1
                }
            ],
            Date = new DateTime(2026, 8, 10),
            DueDate = new DateTime(2026, 8, 9),
        };
        
        var exception = await Assert.ThrowsAsync<InvalidDataException>(() => service.CreatePrescriptionAsync(prescriptionDto, 1));
        Assert.Equal("The due date cannot be earlier than Date.",  exception.Message);
    }
    
    [Fact]
    public async Task CreatePrescriptionAsync_WhenDoctorDoesNotExist_ShouldThrowNotFoundException()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        var options = new DbContextOptionsBuilder<DbContext>().UseSqlite(connection).Options;
        await using var context = new DatabaseContext(options);
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
        var service = new DbService(context);

        var prescriptionDto = new CreatePrescriptionDto()
        {
            Patient = new PatientDto()
            {
                Birthdate = new DateTime(1990, 1, 1),
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe"
            },
            Medicaments =
            [
                new PrescriptionMedicamentDto()
                {
                    Description = "xxx",
                    Dose = 1,
                    IdMedicament = 1
                }
            ],
            Date = new DateTime(2026, 8, 10),
            DueDate = new DateTime(2026, 8, 11),
        };
        
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.CreatePrescriptionAsync(prescriptionDto, 999));
        Assert.Equal("The doctor was not found.",  exception.Message);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_WhenDataIsValid_ShouldSavePrescription()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        var options = new DbContextOptionsBuilder<DbContext>().UseSqlite(connection).Options;
        await using var context = new DatabaseContext(options);
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
        var service = new DbService(context);

        var prescriptionDto = new CreatePrescriptionDto()
        {
            Patient = new PatientDto()
            {
                Birthdate = new DateTime(1990, 1, 1),
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe"
            },
            Medicaments =
            [
                new PrescriptionMedicamentDto()
                {
                    Description = "xxx",
                    Dose = 1,
                    IdMedicament = 1
                }
            ],
            Date = new DateTime(2026, 8, 10),
            DueDate = new DateTime(2026, 8, 11),
        };
        var result = await service.CreatePrescriptionAsync(prescriptionDto, 1);
        Assert.Equal(1, result.IdDoctor);
        Assert.Equal(1, result.PatientId);
        context.ChangeTracker.Clear();
        var savedPrescription = await context.Prescriptions.Include(p => p.PrescriptionsMedicaments)
            .SingleAsync(p => p.IdPrescription == result.IdPrescription, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(prescriptionDto.Patient.IdPatient, savedPrescription.PatientId);
        Assert.Equal(prescriptionDto.Date,  savedPrescription.Date);
        Assert.Equal(prescriptionDto.DueDate, savedPrescription.DueDate);
        var savedMedicament = Assert.Single(savedPrescription.PrescriptionsMedicaments);
        Assert.Equal(prescriptionDto.Medicaments[0].IdMedicament, savedMedicament.IdMedicament);
    }
    
    [Fact]
    public async Task CreatePrescriptionAsync_WhenPatientDoesNotExist_ShouldCreatePatient()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        var options = new DbContextOptionsBuilder<DbContext>().UseSqlite(connection).Options;
        await using var context = new DatabaseContext(options);
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
        var service = new DbService(context);

        var prescriptionDto = new CreatePrescriptionDto()
        {
            Patient = new PatientDto()
            {
                Birthdate = new DateTime(1990, 1, 1),
                IdPatient = 99,
                FirstName = "Test",
                LastName = "Man"
            },
            Medicaments =
            [
                new PrescriptionMedicamentDto()
                {
                    Description = "xxx",
                    Dose = 1,
                    IdMedicament = 1
                }
            ],
            Date = new DateTime(2026, 8, 10),
            DueDate = new DateTime(2026, 8, 11),
        };
        var result = await service.CreatePrescriptionAsync(prescriptionDto, 1);
        Assert.Equal(1, result.IdDoctor);
        context.ChangeTracker.Clear();
        var savedPatient = await context.Patients.Where(p => p.FirstName == prescriptionDto.Patient.FirstName
        && p.LastName == prescriptionDto.Patient.LastName).SingleAsync(cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(savedPatient);
        Assert.Equal(prescriptionDto.Patient.Birthdate, savedPatient.Birthdate);
    }
    
}