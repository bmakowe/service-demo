using AufgabenService.Application.Interfaces;
using AufgabenService.Application.Services;
using AufgabenService.Domain.Interfaces;
using AufgabenService.Domain.Services;
using AufgabenService.Infrastructure.Persistence;
using AufgabenService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AufgabenService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Persistence
            services.AddSingleton<InMemoryContext>();
            services.AddScoped<IAufgabenRepository, AufgabenRepository>();
            
            // Domain Services
            services.AddScoped<AufgabenValidierungsService>();
            
            // Application Services
            services.AddScoped<IAufgabenService, AufgabenAppService>();
            
            return services;
        }
    }
}