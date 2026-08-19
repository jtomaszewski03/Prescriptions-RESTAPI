using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Prescriptions.Api.DTOs;

namespace Prescriptions.Api.Tests.Integration;

public class PatientsEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PatientsEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPatient_WhenPatientExists_ShouldReturnPatient()
    {
        await _factory.ResetDatabaseAsync(TestContext.Current.CancellationToken);
        using var client = _factory.CreateClient();
        
        var response = await client.GetAsync("api/patients/1", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var patient = await response.Content.ReadFromJsonAsync<GetPatientDetailsDto>(TestContext.Current.CancellationToken);
        Assert.NotNull(patient);
        Assert.Equal(1, patient.IdPatient);
    }
    
    [Fact]
    public async Task GetPatient_WhenPatientDoesNotExist_ShouldReturnProblemDetails()
    {
        await _factory.ResetDatabaseAsync(TestContext.Current.CancellationToken);
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("api/patients/999", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
        
        Assert.NotNull(problemDetails);
        Assert.Equal(404, problemDetails.Status);
        Assert.Equal("Patient not found", problemDetails.Detail);
    }
    
}