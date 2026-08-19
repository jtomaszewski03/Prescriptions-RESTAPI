using Prescriptions.Api.DTOs;
using Prescriptions.Api.Models;

namespace Prescriptions.Api.Services;

public interface ITokenService
{
    LoginResponseDto CreateToken(User user);
}