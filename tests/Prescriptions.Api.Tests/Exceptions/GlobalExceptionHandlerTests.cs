using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Prescriptions.Api.Exceptions;

namespace Prescriptions.Api.Tests.Exceptions;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_WhenExceptionIsNotFound_ShouldSetStatusCode404()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        var logger = NullLogger<GlobalExceptionHandler>.Instance;
        var handler = new GlobalExceptionHandler(logger);
        var exception = new NotFoundException("Patient not found");
        
        var handled = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);
        Assert.True(handled);
        Assert.Equal(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_WhenExceptionIsUnexpected_ShouldReturnSafeProblemDetails()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        
        var logger = NullLogger<GlobalExceptionHandler>.Instance;
        var handler = new GlobalExceptionHandler(logger);
        var exception = new Exception("Something went wrong");
        
        var handled = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);
        httpContext.Response.Body.Position = 0;
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(httpContext.Response.Body,
            cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemDetails.Status);
        Assert.Equal("Unexpected server error.", problemDetails.Detail);
        Assert.DoesNotContain(exception.Message, problemDetails.Detail ?? string.Empty);
    }
}