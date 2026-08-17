namespace Prescriptions.Api.DTOs;

public class LoginResponseDto
{
    public string TokenType { get; set; } =  "Bearer";
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}