using Microsoft.Extensions.DependencyInjection;
using AufgabenService.Application.Interfaces;
using AufgabenService.Infrastructure.Persistence;
using AufgabenService.Infrastructure.Persistence.Repositories;

namespace AufgabenService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // In-Memory Datenquelle
            services.AddSingleton<InMemoryContext>();
            
            // Repository-Registrierung
            services.AddScoped<IAufgabenRepository, AufgabenRepository>();
            
            return services;
        }
    }
}