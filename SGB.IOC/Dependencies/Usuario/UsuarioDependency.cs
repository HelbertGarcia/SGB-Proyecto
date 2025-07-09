using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Services.UsuarioServices;
using SGB.Persistence.Interfaces;
using SGB.Persistence.Repositories;
using SGB.Application.Contracts;
namespace SGB.IOC.Dependencies.Usuario
{
    public static class UsuarioDependency
    {
        public static void AddUsuarioDependency(this IServiceCollection services)
        {
            services.AddScoped<IPersonaRepository, PersonaRepository>();
            services.AddTransient<IUsuarioServices, UsuarioService>();

        }















    }
}
