using Microsoft.AspNetCore.Mvc;
using AufgabenService.Application.DTOs;
using AufgabenService.Application.Interfaces;
using AufgabenService.Application.Services;
using AufgabenService.Application.Exceptions;
using AufgabenService.Infrastructure;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// CORS konfigurieren
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5101", "http://localhost:5102", "http://aufgaben-client", "http://pruefung-client")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Application Services registrieren
builder.Services.AddScoped<IAufgabenService, AufgabenService.Application.Services.AufgabenService>();

// Infrastructure einbinden
builder.Services.AddInfrastructure();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Globaler Exception Handler
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (AufgabenService.Application.Exceptions.NotFoundException ex) // Vollständigen Namespace verwenden
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (AufgabenService.Application.Exceptions.ValidationException ex) // Vollständigen Namespace verwenden
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Ein interner Fehler ist aufgetreten." });
        
        // In der Produktionsumgebung würde man hier noch loggen
        Console.Error.WriteLine($"Unhandled exception: {ex}");
    }
});

// API-Endpunkte
app.MapGet("/api/aufgaben", async ([FromServices] IAufgabenService aufgabenService) =>
{
    var aufgaben = await aufgabenService.GetAllAufgabenAsync();
    return Results.Ok(aufgaben);
})
.WithName("GetAufgaben")
.WithOpenApi();

app.MapGet("/api/aufgaben/{id}", async (int id, [FromServices] IAufgabenService aufgabenService) =>
{
    var aufgabe = await aufgabenService.GetAufgabeByIdAsync(id);
    return Results.Ok(aufgabe);
})
.WithName("GetAufgabeById")
.WithOpenApi();

app.MapPost("/api/aufgaben", async (AufgabeErstellenDto aufgabeData, [FromServices] IAufgabenService aufgabenService) =>
{
    var aufgabe = await aufgabenService.CreateAufgabeAsync(aufgabeData);
    return Results.Created($"/api/aufgaben/{aufgabe.Id}", aufgabe);
})
.WithName("CreateAufgabe")
.WithOpenApi();

app.MapPut("/api/aufgaben/{id}", async (int id, AufgabeAktualisierenDto aufgabeData, [FromServices] IAufgabenService aufgabenService) =>
{
    var aufgabe = await aufgabenService.UpdateAufgabeAsync(id, aufgabeData);
    return Results.Ok(aufgabe);
})
.WithName("UpdateAufgabe")
.WithOpenApi();

app.MapDelete("/api/aufgaben/{id}", async (int id, [FromServices] IAufgabenService aufgabenService) =>
{
    await aufgabenService.DeleteAufgabeAsync(id);
    return Results.NoContent();
})
.WithName("DeleteAufgabe")
.WithOpenApi();

app.Run();