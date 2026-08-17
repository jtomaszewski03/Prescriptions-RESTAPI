using Prescriptions.Api.DTOs;
using Prescriptions.Api.Models;

namespace Prescriptions.Api.Services;

public interface IDbService
{
    Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto prescriptionDto, int idDoctor,
        CancellationToken ct);
    Task<GetPatientDetailsDto> GetPatientDetailsAsync(int id, CancellationToken ct);
    Task DeletePatientAsync(int id, CancellationToken ct);
}