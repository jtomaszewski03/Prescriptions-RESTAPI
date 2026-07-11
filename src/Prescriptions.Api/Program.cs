using Microsoft.EntityFrameworkCore;
using Prescriptions.Api.Data;
using Prescriptions.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Database context configuration.
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

// Dependency injection setup.
builder.Services.AddScoped<IDbService, DbService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
