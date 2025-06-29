using Microsoft.EntityFrameworkCore;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Services.LibrosServices;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;

namespace SGB.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- 1. CONFIGURACIÓN DE LA BASE DE DATOS ---
            var connectionString = builder.Configuration.GetConnectionString("SGBDatabase");
            builder.Services.AddDbContext<SGBContext>(options =>
                options.UseSqlServer(connectionString)
            );

            // --- 2. REGISTRO DE DEPENDENCIAS (Módulos Libro y Categoría) ---

            // Repositorios (Scoped: una instancia por petición HTTP)
            builder.Services.AddScoped<ILibroRepository, LibroRepository>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();

            // Servicios (Transient: una nueva instancia cada vez que se solicita)
            builder.Services.AddTransient<ILibroService, LibroService>();
            builder.Services.AddTransient<ICategoriaService, CategoriaService>();

            // --- SERVICIOS ESTÁNDAR DE LA API ---
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // --- CONFIGURACIÓN DEL PIPELINE HTTP ---
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
