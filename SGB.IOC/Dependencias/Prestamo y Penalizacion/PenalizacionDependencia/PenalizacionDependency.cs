using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices;
using SGB.Persistence.Repositories;


namespace SGB.IOC.Dependencias.Prestamo_y_Penalizacion.PenalizacionDependencia
{
    public  static class PenalizacionDependency
    {
        public static void AddPenalizacionDependency( this IServiceCollection  services)
        {
            
            services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            services.AddTransient<IPenalizacionServices, PenalizacionServices>();



            //services.AddScoped<IPrestamosServices, PrestamosServices>();
            //services.AddScoped<IPrestamosRepository, PrestamosRepository>();
        }
    }
}
