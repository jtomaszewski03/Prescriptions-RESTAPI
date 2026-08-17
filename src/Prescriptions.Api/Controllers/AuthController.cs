using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Prescriptions.Api.DTOs;
using Prescriptions.Api.Services;

namespace Prescriptions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserCredentialsDto userCredentialsDto, CancellationToken ct)
    {
        var user = await _authService.RegisterUserAsync(userCredentialsDto, ct);
        return StatusCode(StatusCodes.Status201Created, user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(UserCredentialsDto userCredentialsDto, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(userCredentialsDto, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetUser()
    {
        return Ok(new
        {
            idUser = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
            email = User.FindFirstValue(JwtRegisteredClaimNames.Email),
            role = User.FindFirstValue("role"),
        });
    }
}