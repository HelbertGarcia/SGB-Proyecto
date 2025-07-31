using FluentValidation;
using SGB.Api.Contracts.Mappers;
using SGB.Infraestructure.Loggers;
using SGB.Persistence.Repositories;
using SGB.Api.Services.ConfiguracionServices;
using SGB.Api.Contracts.Repository.Interfaces;
using SGB.Api.Extensions.Mappers.ConfiguracionMapper;
using SGB.Api.Validators.FluentValidator.Configuracion;
using SGB.Api.Contracts.Service.IConfiguracionService;
using SGB.Api.Validators.BusinessValidators.Configuracion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static SGB.Api.Extensions.Loggin.LoggerExtensions;

namespace SGB.IOC.Dependencies.ConfiguracionDependency
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

            services.AddScoped<IConfiguracionService, ConfiguracionService>();

            services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();

            services.AddTransient<IConfiguracionMapper, ConfiguracionMapper>();

            services.AddTransient<IConfiguracionValidator, ConfiguracionValidator>();
          
            services.AddValidatorsFromAssemblyContaining<AddConfiguracionDtoValidator>();

            return services;
        }
    }
}
