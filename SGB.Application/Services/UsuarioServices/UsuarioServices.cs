using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using SGB.Persistence.Interfaces;
using SGB.Persistence.Repositories;
using static SGB.Application.Contracts.Service.IUsuarioServices.IUsuarioServices;

namespace SGB.Application.Services.UsuarioServices
{
    public sealed class UsuarioService : IUsuarioServices
    {
        private readonly IUsuarioService IPersonaRepository;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioService usuarioRepository,
                              ILogger<UsuarioService> logger,
                              IConfiguration configuration)
        {
            IPersonaRepository = usuarioRepository;
            _logger = logger;
            _configuration = configuration;
        }

        

        public async Task<OperationResult> AddUsuarioAsync(SaveUsuarioDto usuarioDto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var UsuarioDto = new UsuarioDto
                {
                    Nombre = usuarioDto.Nombre,
                    Email = usuarioDto.Email,
                    PasswordHash = usuarioDto.PasswordHash,
                    IDRol = usuarioDto.IDRol,
                    FechaCreacion = DateTime.UtcNow
                };

                result = await IPersonaRepository.AddAsync(UsuarioDto);
                _logger.LogInformation("Usuario creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario.");
                result.IsSuccess = false;
                result.Message = "Error al crear el usuario.";
            }

            return result;
        }

        public async Task<OperationResult> UpdateUsuarioAsync(int id, UpdateUsuarioDto usuarioDto, IPersonaRepository personaRepository)
        {
            OperationResult result = new OperationResult();
            try
            {
                var UsuarioDto = new UsuarioDto
                {
                    IDUsuario = id,
                    Nombre = usuarioDto.Nombre,
                    Email = usuarioDto.Email,
                    PasswordHash = usuarioDto.PasswordHash,
                    IDRol = usuarioDto.IDRol,
                    EstaActivo = usuarioDto.EstaActivo,
                    FechaActualizacion = DateTime.UtcNow
                };

                result = await IPersonaRepository.UpdateAsync(UsuarioDto);
                _logger.LogInformation("Usuario actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario.");
                result.IsSuccess = false;
                result.Message = "Error al actualizar el usuario.";
            }

            return result;
        }

        public async Task<OperationResult> DeleteUsuarioAsync(int id, IPersonaRepository personaRepository)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await personaRepository.DeleteAsync(id);
                _logger.LogInformation("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario.");
                result.IsSuccess = false;
                result.Message = "Error al eliminar el usuario.";
            }

            return result;
        }

        public IUsuarioService GetPersonaRepository()
        {
            return IPersonaRepository;
        }

        public async Task<OperationResult> ObtenerDetallesUsuarioAsync(int id, IUsuarioService personaRepository)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await personaRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles del usuario.");
                result.IsSuccess = false;
                result.Message = "Error al obtener detalles del usuario.";
            }

            return result;
        }


        public async Task<OperationResult> BuscarUsuariosAsync(string terminoBusqueda, IUsuarioService personaRepository)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await personaRepository.SearchAsync(terminoBusqueda);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios.");
                result.IsSuccess = false;
                result.Message = "Error al buscar usuarios.";
            }

            return result;
        }

        public IUsuarioService GetIPersonaRepository()
        {
            return IPersonaRepository;
        }

        public async Task<OperationResult> ObtenerTodosLosUsuariosAsync(IUsuarioService personaRepository)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await personaRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                result.IsSuccess = false;
                result.Message = "Error al obtener todos los usuarios.";
            }

            return result;
        }

       







  
    }

}
