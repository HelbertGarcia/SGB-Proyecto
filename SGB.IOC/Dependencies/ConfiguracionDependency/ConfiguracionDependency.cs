using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Mappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Extensions.Mappers.ConfiguracionMapper;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Application.Validators.BusinessValidators.Configuracion;
using SGB.Application.Validators.FluentValidator.Configuracion;
using SGB.Infraestructure.Loggers;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.IOC.Dependencies.ConfiguracionDependency
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SGBContext>(options => options.UseSqlServer(configuration.GetConnectionString("SGBDatabase"))
           );

            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            services.AddScoped<IConfiguracionService, ConfiguracionService>();
            services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
            services.AddTransient<IConfiguracionMapper, ConfiguracionMapper>();
            services.AddTransient<IConfiguracionValidator, ConfiguracionValidator>();

            services.AddLogging();
          
            services.AddValidatorsFromAssemblyContaining<AddConfiguracionDtoValidator>();

            return services;
        }
    }
}
