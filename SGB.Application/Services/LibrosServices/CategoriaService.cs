using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Services.LibrosServices
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILibroRepository _libroRepository;
        private readonly ILogger<CategoriaService> _logger;
        private readonly IConfiguration _configuration;

        public CategoriaService(
            ICategoriaRepository categoriaRepository,
            ILibroRepository libroRepository,
            ILogger<CategoriaService> logger,
            IConfiguration configuration)
        {
            _categoriaRepository = categoriaRepository;
            _libroRepository = libroRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> AddCategoriaAsync(AddCategoriaDto addCategoriaDto)
        {
            try
            {
                try
                {
                    var resultadoExistencia = await _categoriaRepository.ObtenerPorNombreAsync(addCategoriaDto.Nombre);
                    if (resultadoExistencia.Data != null)
                    {
                        return await Task.FromResult(new OperationResult { Success = false, Message = "Ya existe una categoría con ese nombre." });
                    }

                    var nuevaCategoria = new Categoria(addCategoriaDto.Nombre);

                    var resultadoRepo = await _categoriaRepository.AddAsync(nuevaCategoria);
                    if (!resultadoRepo.Success) return resultadoRepo;

                    var categoriaDto = new CategoriaDto(
                        nuevaCategoria.Id,
                        nuevaCategoria.Nombre,
                        nuevaCategoria.EstaActivo
                    );

                    return new OperationResult { Success = true, Data = categoriaDto, Message = "Categoría creada exitosamente." };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear la categoría: {Nombre}", addCategoriaDto?.Nombre);
                    return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoría: {Nombre}", addCategoriaDto?.Nombre);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> UpdateCategoriaAsync(int id, UpdateCategoriaDto updateCategoriaDto)
        {
            try
            {
                var categoriaEntidad = await _categoriaRepository.GetByIdAsync(id);
                if (categoriaEntidad == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] });
                }

                categoriaEntidad.ActualizarNombre(updateCategoriaDto.Nombre);

                return await _categoriaRepository.UpdateAsync(categoriaEntidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoría con ID: {Id}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> DeleteCategoriaAsync(int id)
        {
            try
            {
                var librosConCategoria = await _libroRepository.FindByConditionAsync(l => l.IDCategoria == id && l.EstaActivo);
                if (librosConCategoria.Data is IEnumerable<dynamic> lista && lista.Any())
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = "No se puede eliminar la categoría porque está asignada a uno o más libros activos." });
                }

                return await _categoriaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la categoría con ID: {Id}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetCategoriaAsync(int id)
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
                _logger.LogError(ex, "Error al obtener la categoría con ID: {Id}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetAllCategoriasAsync()
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
                _logger.LogError(ex, "Error al obtener todas las categorías activas.");
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }
    }
}
