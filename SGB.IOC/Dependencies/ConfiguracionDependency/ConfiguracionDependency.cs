using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Mappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Extensions.Mappers.ConfiguracionMapper;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Application.Validators.BusinessValidators.Configuracion;
using SGB.Application.Validators.FluentValidator.Configuracion;
using SGB.Persistence.Repositories;
using SGB.Application.Extensions.Mappers;

namespace SGB.IOC.Dependencies.ConfiguracionDependency
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddConfiguracionDependency(this IServiceCollection service)
        {
            service.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
            service.AddTransient<IConfiguracionService, ConfiguracionService>();

            service.AddTransient<IConfiguracionMapper, ConfiguracionMapper>();

            service.AddTransient<IConfiguracionValidator, ConfiguracionValidator>();
            service.AddValidatorsFromAssemblyContaining<AddConfiguracionDtoValidator>();
            service.AddValidatorsFromAssemblyContaining<UpdateConfiguracionDtoValidator>();
            service.AddValidatorsFromAssemblyContaining<ConfiguracionDtoValidator>();
            service.AddValidatorsFromAssemblyContaining<DeleteConfiguracionDtoValidator>();
            return service;
        }
    }
}
