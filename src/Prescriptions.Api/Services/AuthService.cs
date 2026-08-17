using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prescriptions.Api.Data;
using Prescriptions.Api.DTOs;
using Prescriptions.Api.Exceptions;
using Prescriptions.Api.Models;

namespace Prescriptions.Api.Services;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(DatabaseContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<UserDto> RegisterUserAsync(UserCredentialsDto userCredentialsDto, CancellationToken ct)
    {
        var email = userCredentialsDto.Email.Trim().ToLowerInvariant();
        var emailExists = await _context.Users.AnyAsync(u => u.Email == email, ct);
        if (emailExists)
        {
            throw new ConflictException("User with this email already exists.");
        }

        var user = new User
        {
            Email = email,
            Role = UserRole.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, userCredentialsDto.Password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);

        return new UserDto()
        {
            Email = user.Email,
            IdUser = user.IdUser,
            Role = user.Role.ToString()
        };
    }

    public async Task<LoginResponseDto> LoginAsync(UserCredentialsDto userCredentialsDto, CancellationToken ct)
    {
        var email = userCredentialsDto.Email.Trim().ToLowerInvariant();
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email, ct);

        if (user == null)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var verification = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            userCredentialsDto.Password);

        if (verification == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, userCredentialsDto.Password);
            await _context.SaveChangesAsync(ct);
        }

        return _tokenService.CreateToken(user);
    }
}