using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruefungService.Application.Interfaces;
using PruefungService.Application.Services;
using PruefungService.Domain.Interfaces;
using PruefungService.Domain.Services;
using PruefungService.Infrastructure.ExternalServices;
using PruefungService.Infrastructure.Persistence;
using PruefungService.Infrastructure.Persistence.Repositories;

namespace PruefungService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Persistence
            services.AddSingleton<InMemoryContext>();
            services.AddScoped<IPruefungRepository, PruefungRepository>();
            
            // Domain Services
            services.AddScoped<PruefungValidierungsService>();
            
            // Application Services
            services.AddScoped<IPruefungAppService, PruefungAppService>();
            
            // External Services
            services.AddHttpClient<IAufgabenService, AufgabenServiceClient>(client =>
            {
                // In Docker-Umgebung: Feste URL innerhalb des Docker-Netzwerks verwenden
                client.BaseAddress = new Uri("http://aufgaben-api:8080/");
                Console.WriteLine($"Setting AufgabenAPI BaseAddress to: http://aufgaben-api:8080/");
            });
            
            return services;
        }
    }
}