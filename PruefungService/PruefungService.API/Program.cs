using Microsoft.AspNetCore.Mvc;
using PruefungService.Application.DTOs;
using PruefungService.Application.Exceptions;
using PruefungService.Application.Interfaces;
using PruefungService.Infrastructure;

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

// Onion Architecture Setup - Infrastrukturschicht registrieren
builder.Services.AddInfrastructure(builder.Configuration);

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

// Global exception handling
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (NotFoundException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        // Log the error
        Console.WriteLine($"Unhandled exception: {ex}");
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Ein interner Serverfehler ist aufgetreten." });
    }
});

// API-Endpunkte konfigurieren
ConfigureApiEndpoints(app);

app.Run();

void ConfigureApiEndpoints(WebApplication app)
{
    // Alle Prüfungen abrufen
    app.MapGet("/api/pruefung", async (IPruefungAppService pruefungService) =>
    {
        var pruefungen = await pruefungService.GetAllePruefungenAsync();
        return Results.Ok(pruefungen);
    })
    .WithName("GetPruefungen")
    .WithOpenApi();

    // Eine spezifische Prüfung abrufen
    app.MapGet("/api/pruefung/{id}", async (int id, IPruefungAppService pruefungService) =>
    {
        var pruefung = await pruefungService.GetPruefungByIdAsync(id);
        if (pruefung == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(pruefung);
    })
    .WithName("GetPruefungById")
    .WithOpenApi();

    // Aufgaben für eine Prüfung abrufen
    app.MapGet("/api/pruefung/{id}/aufgaben", async (int id, IPruefungAppService pruefungService) =>
    {
        try
        {
            var aufgaben = await pruefungService.GetAufgabenFuerPruefungAsync(id);
            return Results.Ok(aufgaben);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tasks: {ex.Message}");
            Console.WriteLine($"Error details: {ex}");
            return Results.Problem($"Fehler beim Abrufen der Aufgaben: {ex.Message}");
        }
    })
    .WithName("GetAufgabenFuerPruefung")
    .WithOpenApi();

    // Alle Aufgaben abrufen (vom AufgabenService)
    app.MapGet("/api/aufgaben", async (IPruefungAppService pruefungService) =>
    {
        var aufgaben = await pruefungService.GetAlleAufgabenAsync();
        return Results.Ok(aufgaben);
    })
    .WithName("GetAllAufgaben")
    .WithOpenApi();

    // Neue Prüfung erstellen
    app.MapPost("/api/pruefung", async (PruefungErstellenDto pruefungDto, IPruefungAppService pruefungService) =>
    {
        try
        {
            var neuePruefung = await pruefungService.ErstellePruefungAsync(pruefungDto);
            return Results.Created($"/api/pruefung/{neuePruefung.Id}", neuePruefung);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("CreatePruefung")
    .WithOpenApi();

    // Prüfungsdaten aktualisieren
    app.MapPut("/api/pruefung/{id}", async (int id, PruefungAktualisierenDto pruefungDto, IPruefungAppService pruefungService) =>
    {
        try
        {
            var aktualisiert = await pruefungService.AktualisierePruefungAsync(id, pruefungDto);
            if (aktualisiert == null)
            {
                return Results.NotFound("Prüfung nicht gefunden");
            }
            return Results.Ok(aktualisiert);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("UpdatePruefung")
    .WithOpenApi();

    // Aufgaben einer Prüfung aktualisieren
    app.MapPut("/api/pruefung/{id}/aufgaben", async (int id, AufgabenZuweisenDto aufgabenDto, IPruefungAppService pruefungService) =>
    {
        try
        {
            var aktualisiert = await pruefungService.WeiseAufgabenZuAsync(id, aufgabenDto);
            if (aktualisiert == null)
            {
                return Results.NotFound("Prüfung nicht gefunden");
            }
            return Results.Ok(aktualisiert);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("AssignAufgabenToPruefung")
    .WithOpenApi();

    // Prüfung löschen
    app.MapDelete("/api/pruefung/{id}", async (int id, IPruefungAppService pruefungService) =>
    {
        var erfolg = await pruefungService.LoeschePruefungAsync(id);
        if (!erfolg)
        {
            return Results.NotFound("Prüfung nicht gefunden");
        }
        return Results.NoContent();
    })
    .WithName("DeletePruefung")
    .WithOpenApi();
}