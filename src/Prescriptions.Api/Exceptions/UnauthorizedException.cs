namespace Prescriptions.Api.Exceptions;

public class UnauthorizedException(string message) : Exception(message);