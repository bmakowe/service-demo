using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PruefungService.Client;
using PruefungService.Client.Services.Implementations;
using PruefungService.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API URL aus der Umgebungsvariable oder Fallback
var apiUrl = builder.Configuration["PruefungApiUrl"] ?? "http://localhost:5002";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

// Services registrieren
builder.Services.AddScoped<IPruefungDataService, PruefungDataService>();

await builder.Build().RunAsync();