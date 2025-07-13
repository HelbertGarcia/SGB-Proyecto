using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Application.Services.LibrosServices
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ICategoriaBusinessValidator _categoriaValidator; 
        private readonly ILogger<CategoriaService> _logger;
        private readonly IConfiguration _configuration;

        public CategoriaService(
            ICategoriaRepository categoriaRepository,
            ICategoriaBusinessValidator categoriaValidator,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _categoriaRepository = categoriaRepository;
            _categoriaValidator = categoriaValidator;
            _logger = loggerFactory.CreateLogger<CategoriaService>();
            _configuration = configuration;
        }

        public async Task<OperationResult<CategoriaDto>> AddAsync(AddCategoriaDto dto)
        {
            try
            {
                var validationResult = await _categoriaValidator.ValidateForAddAsync(dto);
                if (!validationResult.IsSuccess)
                {
                    return OperationResult<CategoriaDto>.Failure(validationResult.Message);
                }

                var nuevaCategoria = new Categoria(dto.Nombre);

                var repoResult = await _categoriaRepository.AddAsync(nuevaCategoria);
                if (!repoResult.IsSuccess)
                    return OperationResult<CategoriaDto>.Failure(repoResult.Message);

                var categoriaDto = new CategoriaDto(
                    repoResult.Data.Id,
                    repoResult.Data.Nombre,
                    repoResult.Data.EstaActivo
                );

                return OperationResult<CategoriaDto>.Success(categoriaDto, "Categoría creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al crear categoría: {Nombre}", dto?.Nombre);
                return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<CategoriaDto>> UpdateAsync(int id, UpdateCategoriaDto dto)
        {
            try
            {
                var categoriaEntidad = await _categoriaRepository.GetByIdAsync(id);
                if (!categoriaEntidad.IsSuccess || categoriaEntidad.Data == null)
                {
                    return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);
                }

                categoriaEntidad.Data.ActualizarNombre(dto.Nombre);

                var repoResult = await _categoriaRepository.UpdateAsync(categoriaEntidad.Data);
                if (!repoResult.IsSuccess)
                    return OperationResult<CategoriaDto>.Failure(repoResult.Message);

                var categoriaDto = new CategoriaDto(repoResult.Data.Id, repoResult.Data.Nombre, repoResult.Data.EstaActivo);
                return OperationResult<CategoriaDto>.Success(categoriaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al actualizar categoría con ID: {ID}", id);
                return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var validationResult = await _categoriaValidator.ValidateForDeleteAsync(id);
                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                return await _categoriaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar categoría con ID: {ID}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<CategoriaDto>> GetByIdAsync(int id)
        {
            var result = await _categoriaRepository.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);

            var dto = new CategoriaDto(result.Data.Id, result.Data.Nombre, result.Data.EstaActivo);
            return OperationResult<CategoriaDto>.Success(dto);
        }

        public async Task<OperationResult<IEnumerable<CategoriaDto>>> GetAllAsync()
        {
            var result = await _categoriaRepository.GetAllAsync();
            if (!result.IsSuccess)
                return OperationResult<IEnumerable<CategoriaDto>>.Failure(result.Message);

            var dtoList = result.Data.Select(c => new CategoriaDto(c.Id, c.Nombre, c.EstaActivo));
            return OperationResult<IEnumerable<CategoriaDto>>.Success(dtoList);
        }
    }
}