using AufgabenService.Application.DTOs;
using AufgabenService.Application.Exceptions;
using AufgabenService.Application.Interfaces;
using AufgabenService.Infrastructure;

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
    // Alle Aufgaben abrufen
    app.MapGet("/api/aufgaben", async (IAufgabenService aufgabenService) =>
    {
        var aufgaben = await aufgabenService.GetAlleAufgabenAsync();
        return Results.Ok(aufgaben);
    })
    .WithName("GetAufgaben")
    .WithOpenApi();

    // Eine spezifische Aufgabe abrufen
    app.MapGet("/api/aufgaben/{id}", async (int id, IAufgabenService aufgabenService) =>
    {
        var aufgabe = await aufgabenService.GetAufgabeByIdAsync(id);
        if (aufgabe == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(aufgabe);
    })
    .WithName("GetAufgabeById")
    .WithOpenApi();

    // Neue Aufgabe erstellen
    app.MapPost("/api/aufgaben", async (AufgabeErstellenDto aufgabeDto, IAufgabenService aufgabenService) =>
    {
        try
        {
            var neueAufgabe = await aufgabenService.ErstelleAufgabeAsync(aufgabeDto);
            return Results.Created($"/api/aufgaben/{neueAufgabe.Id}", neueAufgabe);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("CreateAufgabe")
    .WithOpenApi();

    // Bestehende Aufgabe aktualisieren
    app.MapPut("/api/aufgaben/{id}", async (int id, AufgabeAktualisierenDto aufgabeDto, IAufgabenService aufgabenService) =>
    {
        try
        {
            var aktualisierteAufgabe = await aufgabenService.AktualisiereAufgabeAsync(id, aufgabeDto);
            if (aktualisierteAufgabe == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(aktualisierteAufgabe);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("UpdateAufgabe")
    .WithOpenApi();

    // Aufgabe löschen
    app.MapDelete("/api/aufgaben/{id}", async (int id, IAufgabenService aufgabenService) =>
    {
        var erfolg = await aufgabenService.LoescheAufgabeAsync(id);
        if (!erfolg)
        {
            return Results.NotFound();
        }
        return Results.NoContent();
    })
    .WithName("DeleteAufgabe")
    .WithOpenApi();
}