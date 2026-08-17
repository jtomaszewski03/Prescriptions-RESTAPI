using Microsoft.AspNetCore.Authorization;
using Prescriptions.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Prescriptions.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [Authorize(Roles = "Doctor,Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var patientDetails = await _dbService.GetPatientDetailsAsync(id);
        return Ok(patientDetails);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        await _dbService.DeletePatientAsync(id);
        return NoContent();
    }
}