
using Microsoft.EntityFrameworkCore;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Prestamos;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PrestamoServices;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using SGB.Persistence.Repositories;

namespace SGB.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add services to the container.

            // --- 1. CONFIGURACIÓN DE LA BASE DE DATOS ---
            var connectionString = builder.Configuration.GetConnectionString("SGBDatabase");
            builder.Services.AddDbContext<SGBContext>(options =>
                options.UseSqlServer(connectionString)
            );




            // Prestamos
            builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            builder.Services.AddTransient<IPrestamosServices, PrestamoService>();

            // Penalizacion
            builder.Services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            builder.Services.AddTransient<IPenalizacionServices, PenalizacionServices>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
