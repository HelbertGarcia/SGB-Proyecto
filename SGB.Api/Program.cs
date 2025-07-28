using Microsoft.EntityFrameworkCore;
using SGB.Infraestructure.Loggers;
using SGB.IOC.Dependencies.ConfiguracionDependency;
using SGB.Persistence.Context;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SGBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SGBDatabase")));

            builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            builder.Services.AddDependencies(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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