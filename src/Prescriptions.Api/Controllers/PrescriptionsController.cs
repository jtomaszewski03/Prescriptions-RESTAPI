using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Prescriptions.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
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

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto request, CancellationToken ct)
        {
            if (!int.TryParse(User.FindFirstValue("IdDoctor"), out var idDoctor))
            {
                return Forbid();
            }
            
            await _dbService.CreatePrescriptionAsync(request, idDoctor, ct);
            return Created();
        }
    }
}