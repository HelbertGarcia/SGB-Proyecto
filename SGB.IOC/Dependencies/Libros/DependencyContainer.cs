using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Mappers.CategoriaMapper;
using SGB.Application.Contracts.Mappers.LibroMapper;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Extensions.Mappers.CategoriaMapper;
using SGB.Application.Extensions.Mappers.LibroMapper;
using SGB.Application.Services.LibrosServices;
using SGB.Application.Validators.BusinessValidators;
using SGB.Application.Validators.FluentValidators.Libros;
using SGB.Infraestructure.Loggers;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.IOC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SGBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SGBDatabase"))
            );

            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();

            services.AddTransient<ILibroBusinessValidator, LibroBusinessValidator>();
            services.AddTransient<ILibroService, LibroService>();

            services.AddTransient<ICategoriaBusinessValidator, CategoriaBusinessValidator>();
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<ICategoriaMapper, CategoriaMapper>();
            services.AddTransient<ILibroMapper, LibroMapper>();

            services.AddValidatorsFromAssemblyContaining<AddLibroDtoValidator>();

            return services;
        }
    }
}
