using Microsoft.EntityFrameworkCore;
using SGB.Persistence.Context;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Persistence.Repositories;
using SGB.IOC.Dependencies.ConfiguracionDependency;
using SGB.Application.Contracts.Repository.Interfaces;


namespace SGB.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependencies(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers();
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