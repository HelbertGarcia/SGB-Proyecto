
using Microsoft.EntityFrameworkCore;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices;

using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using SGB.IOC.Dependencias.Prestamo_y_Penalizacion.PenalizacionDependencia;
using SGB.IOC.Dependencias.Prestamo_y_Penalizacion.PrestamoDependencia;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto.Validators;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatosDto;



namespace SGB.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add services to the container.

            // --. CONFIGURACIÓN DE LA BASE DE DATOS ---
            var connectionString = builder.Configuration.GetConnectionString("SGBDatabase");
            builder.Services.AddDbContext<SGBContext>(options =>
                options.UseSqlServer(connectionString)
            );

           

            //registrar dependencias de Prestamo y Penalizacion
            builder.Services.AddPenalizacionDependency();
            builder.Services.AddPrestamoDependency();



            /*
            //prestamo 
            builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            builder.Services.AddTransient<IPrestamosServices,PrestamoService>();

            // Penalizacion
            builder.Services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            builder.Services.AddTransient<IPenalizacionServices, PenalizacionService>();
            */
            

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
