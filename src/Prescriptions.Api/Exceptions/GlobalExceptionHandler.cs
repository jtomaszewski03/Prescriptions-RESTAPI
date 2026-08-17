using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Prescriptions.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken
    )
    {
        var (statusCode, title, detail) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, "Not found", exception.Message),
            InvalidDataException => (HttpStatusCode.BadRequest, "Invalid request", exception.Message),
            ConflictException => (HttpStatusCode.Conflict, "Conflict", exception.Message),
            UnauthorizedException => (HttpStatusCode.Unauthorized, "Unauthorized", exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Internal server error", "Unexpected server error.")
        };
        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception during request {TraceId}: {Message}",
                httpContext.TraceIdentifier, exception.Message);
        }
        else
        {
            _logger.LogInformation("{ExceptionType} during request: {TraceId}: {Message}", exception.GetType().Name,
                httpContext.TraceIdentifier, exception.Message);
        }

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Extensions =
            {
                { "traceId", httpContext.TraceIdentifier },
            }
        };
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}