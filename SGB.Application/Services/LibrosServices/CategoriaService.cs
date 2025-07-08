using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Extensions;
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

        public async Task<OperationResult> AddAsync(AddCategoriaDto dto)
        {
            try
            {
                var validationResult = await _categoriaValidator.ValidateForAddAsync(dto);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                var nuevaCategoria = new Categoria(dto.Nombre);
                var repoResult = await _categoriaRepository.AddAsync(nuevaCategoria);
                if (!repoResult.Success) return repoResult;

                var categoriaDto = new CategoriaDto(nuevaCategoria.Id, nuevaCategoria.Nombre, nuevaCategoria.EstaActivo);
                return new OperationResult { Success = true, Data = categoriaDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al crear categoría: {Nombre}", dto?.Nombre);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, UpdateCategoriaDto dto)
        {
            try
            {
                var categoriaEntidad = await _categoriaRepository.GetByIdAsync(id);
                if (categoriaEntidad == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] });
                }

                categoriaEntidad.ActualizarNombre(dto.Nombre);
                return await _categoriaRepository.UpdateAsync(categoriaEntidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al actualizar categoría con ID: {ID}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var validationResult = await _categoriaValidator.ValidateForDeleteAsync(id);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                return await _categoriaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar categoría con ID: {ID}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] });
                }

                var categoriaDto = new CategoriaDto(
                    categoria.Id,
                    categoria.Nombre,
                    categoria.EstaActivo
                );
                return new OperationResult { Success = true, Data = categoriaDto };
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithConfigurationMessage(_configuration, ex, "ErrorMessages:Categorias:GetById", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetAllAsync()
        {
            try
            {
                var resultadoRepo = await _categoriaRepository.FindByConditionAsync(c => c.EstaActivo);
                if (!resultadoRepo.Success) return resultadoRepo;

                var listaEntidades = (IEnumerable<Categoria>)resultadoRepo.Data;

                var listaDto = listaEntidades.Select(c => new CategoriaDto(
                    c.Id,
                    c.Nombre,
                    c.EstaActivo
                )).ToList();

                return new OperationResult { Success = true, Data = listaDto };
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithConfigurationMessage(_configuration, ex, "ErrorMessages:Categorias:GetAll");
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }
    }
}