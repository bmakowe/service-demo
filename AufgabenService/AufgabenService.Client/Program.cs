using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AufgabenService.Client;
using AufgabenService.Client.Services.Implementations;
using AufgabenService.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API URL aus der Umgebungsvariable oder Fallback
var apiUrl = builder.Configuration["AufgabenApiUrl"] ?? "http://localhost:5001";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
builder.Services.AddScoped<IAufgabenDataService, AufgabenDataService>();

await builder.Build().RunAsync();