using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Application.Validators.BusinessValidators.Configuracion;
using SGB.Application.Validators.FluentValidator.Configuracion;
using SGB.Persistence.Interfaces;
using SGB.Persistence.Repositories;
using FluentValidation;

namespace SGB.IOC.Dependencies.ConfiguracionDependency
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddConfiguracionDependency(this IServiceCollection service)
        {
            service.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
            service.AddTransient<IConfiguracionService, ConfiguracionService>();

            service.AddTransient<IConfiguracionValidator, ConfiguracionValidator>();
            service.AddValidatorsFromAssemblyContaining<AddConfiguracionDtoValidator>();
            service.AddValidatorsFromAssemblyContaining<UpdateConfiguracionDtoValidator>();
            service.AddValidatorsFromAssemblyContaining<GetConfiguracionDtoValidator>();
            service.AddValidatorsFromAssemblyContaining<DeleteConfiguracionDtoValidator>();
            return service;
        }
    }
}
