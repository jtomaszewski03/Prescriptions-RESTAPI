using Prescriptions.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Prescriptions.Api.DTOs;
using Prescriptions.Api.Exceptions;

namespace Prescriptions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public PrescriptionsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto request)
        {
            try
            {
                await _dbService.CreatePrescriptionAsync(request);
                return Created();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
