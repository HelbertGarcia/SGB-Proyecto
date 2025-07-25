using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatosDto;

using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Base.ValidatorServices.Prestamos;
using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Extensions.Mappers.PrestamosMapper;
using SGB.Application.Loggers;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatorsDto;
using SGB.Infraestructure.Loggers;



namespace SGB.IOC.Dependencias.Prestamo_y_Penalizacion.PrestamoDependencia
{
    public static class PrestamoDependency
    {
        public static void AddPrestamoDependency(this IServiceCollection services)
        {

          
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            services.AddTransient<IPrestamosServices, PrestamoService>();


            // Validadores DTOs con FluentValidation
            services.AddScoped<IValidator<AddPrestamoDto>, AddPrestamoDtoValidator>();
            services.AddScoped<IValidator<UpdatePrestamoDto>, UpdatePrestamoDtoValidator>();
            services.AddScoped<IValidator<DiseblePrestamoDto>, DisablePrestamoDtoValidator>();
            services.AddScoped<IValidator<RegistrarDevolucionDto>, RegistrarDevolucionValidatorDto>();




            // Validator Services (Reglas de negocio)
            services.AddScoped<IPrestamoBusinessValidator, PrestamoBusinessValidator>();

            services.AddTransient<IPrestamoMapper, PrestamoMapper>();

            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<IAppLogger<PrestamoBusinessValidator>, AppLogger<PrestamoBusinessValidator>>();



        }
    }
}
