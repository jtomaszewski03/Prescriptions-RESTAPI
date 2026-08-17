using Prescriptions.Api.DTOs;

namespace Prescriptions.Api.Services;

public interface IAuthService
{
    Task<UserDto> RegisterUserAsync(UserCredentialsDto registrationCredentialsDto, CancellationToken ct);
    Task<LoginResponseDto> LoginAsync(UserCredentialsDto registrationCredentialsDto, CancellationToken ct);
}