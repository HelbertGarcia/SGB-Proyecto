using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Services.LibrosServices;
using SGB.Application.Validators.BusinessValidators;
using SGB.Application.Validators.FluentValidators.Libros; 
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using FluentValidation;

namespace SGB.IOC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SGBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SGBDatabase"))
            );

            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();

            services.AddTransient<ILibroBusinessValidator, LibroBusinessValidator>();
            services.AddTransient<ILibroService, LibroService>();

            services.AddTransient<ICategoriaBusinessValidator, CategoriaBusinessValidator>();
            services.AddTransient<ICategoriaService, CategoriaService>();

            services.AddValidatorsFromAssemblyContaining<AddLibroDtoValidator>();

            return services;
        }
    }
}
