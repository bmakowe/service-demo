using Microsoft.Extensions.DependencyInjection;
using PruefungService.Application.Interfaces;
using PruefungService.Infrastructure.Persistence;
using PruefungService.Infrastructure.Persistence.Repositories;
using PruefungService.Infrastructure.Services;

namespace PruefungService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // In-Memory Datenquelle
            services.AddSingleton<InMemoryContext>();
            
            // Repository-Registrierung - explizite Namensraumangabe
            services.AddScoped<IPruefungRepository, PruefungRepository>();
            
            // AufgabenServiceClient registrieren
            services.AddHttpClient<IAufgabenServiceClient, AufgabenServiceClient>(client =>
            {
                client.BaseAddress = new Uri("http://aufgaben-api:8080/");
            });
            
            return services;
        }
    }
}