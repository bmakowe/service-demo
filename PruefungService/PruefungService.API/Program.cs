using Microsoft.AspNetCore.Mvc;
using PruefungService.Application.DTOs;
using PruefungService.Application.Interfaces;
using PruefungService.Application.Services;
using PruefungService.Application.Exceptions; // Explizit den Exceptions-Namespace importieren
using PruefungService.Infrastructure;
using System.Net;
using System.Net.Http.Json;

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
builder.Services.AddScoped<IPruefungService, PruefungService.Application.Services.PruefungService>();

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
    catch (PruefungService.Application.Exceptions.NotFoundException ex) // Vollständigen Namespace verwenden
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (PruefungService.Application.Exceptions.ValidationException ex) // Vollständigen Namespace verwenden
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
app.MapGet("/api/pruefung", async ([FromServices] IPruefungService pruefungService) =>
{
    var pruefungen = await pruefungService.GetAllPruefungenAsync();
    return Results.Ok(pruefungen);
})
.WithName("GetPruefungen")
.WithOpenApi();

app.MapGet("/api/pruefung/{id}", async (int id, [FromServices] IPruefungService pruefungService) =>
{
    var pruefung = await pruefungService.GetPruefungByIdAsync(id);
    return Results.Ok(pruefung);
})
.WithName("GetPruefungById")
.WithOpenApi();

app.MapGet("/api/pruefung/{id}/aufgaben", async (int id, [FromServices] IPruefungService pruefungService) =>
{
    var aufgaben = await pruefungService.GetAufgabenForPruefungAsync(id);
    return Results.Ok(aufgaben);
})
.WithName("GetAufgabenFuerPruefung")
.WithOpenApi();

// Endpunkt für die Abfrage aller Aufgaben vom AufgabenService
app.MapGet("/api/aufgaben", async ([FromServices] IAufgabenServiceClient aufgabenServiceClient) =>
{
    var aufgaben = await aufgabenServiceClient.GetAllAufgabenAsync();
    return Results.Ok(aufgaben);
})
.WithName("GetAllAufgaben")
.WithOpenApi();

app.MapPost("/api/pruefung", async (PruefungErstellenDto pruefungData, [FromServices] IPruefungService pruefungService) =>
{
    var pruefung = await pruefungService.CreatePruefungAsync(pruefungData);
    return Results.Created($"/api/pruefung/{pruefung.Id}", pruefung);
})
.WithName("CreatePruefung")
.WithOpenApi();

app.MapPut("/api/pruefung/{id}", async (int id, PruefungAktualisierenDto pruefungData, [FromServices] IPruefungService pruefungService) =>
{
    var pruefung = await pruefungService.UpdatePruefungAsync(id, pruefungData);
    return Results.Ok(pruefung);
})
.WithName("UpdatePruefung")
.WithOpenApi();

app.MapPut("/api/pruefung/{id}/aufgaben", async (int id, AufgabenZuweisenDto aufgabenData, [FromServices] IPruefungService pruefungService) =>
{
    var pruefung = await pruefungService.UpdatePruefungAufgabenAsync(id, aufgabenData);
    return Results.Ok(pruefung);
})
.WithName("AssignAufgabenToPruefung")
.WithOpenApi();

app.MapDelete("/api/pruefung/{id}", async (int id, [FromServices] IPruefungService pruefungService) =>
{
    await pruefungService.DeletePruefungAsync(id);
    return Results.NoContent();
})
.WithName("DeletePruefung")
.WithOpenApi();

app.Run();