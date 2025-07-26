using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Base;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Application.interfaces.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;


namespace SGB.Application.Services.UsuarioServices
{
    public sealed class UsuarioService : IUsuarioServices
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IConfiguration _configuration;
        private IUsuarioServices @object;
        private ILoggerFactory loggerFactory;

        public UsuarioService(
           
            IPersonaRepository personaRepository,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _personaRepository = personaRepository;
            _logger = loggerFactory.CreateLogger<UsuarioService>();
            _configuration = configuration;
        }

        public UsuarioService(IUsuarioServices @object, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this.@object = @object;
            this.loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        public async Task<OperationResult<UsuarioDto>> AddAsync(SaveUsuarioDto dto)
        {

            try
            {
                if (dto is null)
                    return OperationResult<UsuarioDto>.Failure("Datos nulos.");

                var usuarioEntity = new UsuarioDto
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    PasswordHash = dto.PasswordHash,
                    IDRol = dto.IDRol,
                    FechaCreacion = DateTime.UtcNow,
                    EstaActivo = true
                };


                var createdEntity = await _personaRepository.AddAsync(usuarioEntity);


                var usuario = new UsuarioDto
                {
                    IDUsuario = createdEntity.IDUsuario,
                    Nombre = createdEntity.Nombre,
                    Email = createdEntity.Email,
                    IDRol = createdEntity.IDRol,
                    FechaCreacion = createdEntity.FechaCreacion,
                    EstaActivo = createdEntity.EstaActivo
                };

                return OperationResult<UsuarioDto>.Success(usuario, "Usuario creado exitosamente.");
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

                var result = await _personaRepository.UpdateAsync(usuario);
                if (!result.IsSuccess)
                    return OperationResult<UsuarioDto>.Failure(result.Message);

                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                
                return OperationResult<UsuarioDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _personaRepository.DeleteAsync(id);
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
                var result = await _personaRepository.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return OperationResult<UsuarioDto>.Failure(result.Message);

                return await GetByIdAsync(id);
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
                var result = await _personaRepository.GetAllAsync();
                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<UsuarioDto>>.Failure(result.Message);

                return await _personaRepository.ObtenerTodosConDetallesAsync();
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

                var result = await _personaRepository.SearchAsync(termino);
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

        public Task<OperationResult<IEnumerable<UsuarioDto>>>GetAllUsuario()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<SaveUsuarioDto>> AddAsync(UsuarioDto dto)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<SaveUsuarioDto>> IBaseService<UsuarioDto, UpdateUsuarioDto, SaveUsuarioDto>.UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<IEnumerable<SaveUsuarioDto>>> IBaseService<UsuarioDto, UpdateUsuarioDto, SaveUsuarioDto>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<SaveUsuarioDto>> IBaseService<UsuarioDto, UpdateUsuarioDto, SaveUsuarioDto>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
