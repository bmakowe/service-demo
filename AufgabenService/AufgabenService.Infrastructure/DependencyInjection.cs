using AufgabenService.Application.Interfaces;
using AufgabenService.Application.Services;
using AufgabenService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AufgabenService.Application.Mapping;

namespace AufgabenService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddSingleton<IAufgabenRepository, AufgabenRepository>();

            // Services
            services.AddScoped<IAufgabenService, AufgabenAppService>();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}