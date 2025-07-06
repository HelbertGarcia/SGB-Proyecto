using SGB.Persistence.Interfaces;
using SGB.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Services.ConfiguracionServices;

namespace SGB.IOC.Dependencies.ConfiguracionDependency
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddConfiguracionDependency(this IServiceCollection service)
        {
            service.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
            service.AddTransient<IConfiguracionService, ConfiguracionService>();
            return service;
        }
    }
}
