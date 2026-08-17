using Prescriptions.Api.DTOs;
using Prescriptions.Api.Models;

namespace Prescriptions.Api.Services;

public interface IDbService
{
    Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto prescriptionDto, int idDoctor);
    Task<GetPatientDetailsDto> GetPatientDetailsAsync(int id);
    Task DeletePatientAsync(int id);
}
