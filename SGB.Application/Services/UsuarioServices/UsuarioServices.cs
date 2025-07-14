using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using SGB.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGB.Application.Services.UsuarioServices
{
    public sealed class UsuarioService : IUsuarioServices
    {
        private readonly IUsuarioServices _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IConfiguration _configuration;

        public UsuarioService(
            IUsuarioServices usuarioRepository,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _logger = loggerFactory.CreateLogger<UsuarioService>();
            _configuration = configuration;
        }

        public async Task<OperationResult<UsuarioDto>> AddUsuarioAsync(SaveUsuarioDto dto)
        {
            try
            {
                if (dto is null)
                    return OperationResult<UsuarioDto>.Failure("Datos nulos.");

                var usuario = new UsuarioDto
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    PasswordHash = dto.PasswordHash,
                    IDRol = dto.IDRol,
                    FechaCreacion = DateTime.UtcNow
                };

                var result = await _usuarioRepository.AddAsync(usuario);
                if (!result.IsSuccess)
                    return OperationResult<UsuarioDto>.Failure(result.Message);

                return OperationResult<UsuarioDto>.Success(result.Data!, "Usuario creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el usuario: {Email}", dto?.Email);
                return OperationResult<UsuarioDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<UsuarioDto>> UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            try
            {
                if (dto is null)
                    return OperationResult<UsuarioDto>.Failure("Datos nulos.");

                var usuario = new UsuarioDto
                {
                    IDUsuario = id,
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    PasswordHash = dto.PasswordHash,
                    IDRol = dto.IDRol,
                    EstaActivo = dto.EstaActivo,
                    FechaActualizacion = DateTime.UtcNow
                };

                var result = await _usuarioRepository.UpdateAsync(usuario);
                if (!result.IsSuccess)
                    return OperationResult<UsuarioDto>.Failure(result.Message);

                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {ID}", id);
                return OperationResult<UsuarioDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _usuarioRepository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return OperationResult<bool>.Failure(result.Message);

                return OperationResult<bool>.Success(true, "Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {ID}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<UsuarioDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _usuarioRepository.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return OperationResult<UsuarioDto>.Failure(result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles del usuario con ID: {ID}", id);
                return OperationResult<UsuarioDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<IEnumerable<UsuarioDto>>> GetAllAsync()
        {
            try
            {
                var result = await _usuarioRepository.GetAllAsync();
                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<UsuarioDto>>.Failure(result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                return OperationResult<IEnumerable<UsuarioDto>>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<IEnumerable<UsuarioDto>>> BuscarUsuariosAsync(string termino)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termino))
                    return await GetAllAsync();

                var result = await _usuarioRepository.SearchAsync(termino);
                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<UsuarioDto>>.Failure(result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios con término: {Termino}", termino);
                return OperationResult<IEnumerable<UsuarioDto>>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public Task<object?> GetAllUsuario()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(UsuarioDto usuario)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<UsuarioDto>> AddAsync(SaveUsuarioDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<UsuarioDto>>> SearchAsync(string termino)
        {
            throw new NotImplementedException();
        }
    }
}
