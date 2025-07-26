using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Services.UsuarioServices;
using SGB.Infraestructure.Loggers;
using SGB.Persistence.Context;

using SGB.Persistence.Repositories;
using static SGB.Application.Extensions.SGB.Application.Extensions.Loggin.LoggerExtensions;
namespace SGB.IOC.Dependencies.Usuario
{
    public static class UsuarioDependency
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
           


            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            services.AddScoped<IPersonaRepository, PersonaRepository>();
            services.AddTransient<IUsuarioServices, UsuarioService>();

            return services;








        }
    }
}
