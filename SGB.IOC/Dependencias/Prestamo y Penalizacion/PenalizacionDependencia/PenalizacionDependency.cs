using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto.Validators;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices;
using SGB.Persistence.Repositories;
using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Extensions.Mappers.PenalizacionesMapper;
using SGB.Application.Extensions.Mappers.PrestamosMapper;



namespace SGB.IOC.Dependencias.Prestamo_y_Penalizacion.PenalizacionDependencia
{
    public  static class PenalizacionDependency
    {
        public static void AddPenalizacionDependency( this IServiceCollection  services)
        {
            
            services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            services.AddTransient<IPenalizacionServices, PenalizacionService>();


            // Validadores DTOs con FluentValidation
            services.AddScoped<IValidator<AddPenalizacionDto>, AddPenalizacionDtoValidator>();
            services.AddScoped<IValidator<UpdatePenalizacionDto>, UpdatePenalizacionDtoValidator>();
            services.AddScoped<IValidator<DisablePenalizacionDto>, DisablePenalizacionDtoValidator>();

            // Validator Services (Reglas de negocio)

            services.AddScoped<IPenalizacionBusinessValidator, PenalizacionBusinessValidator>();


            services.AddTransient<IPenalizacionMapper, PenalizacionMapper>();

        }
    }
}
