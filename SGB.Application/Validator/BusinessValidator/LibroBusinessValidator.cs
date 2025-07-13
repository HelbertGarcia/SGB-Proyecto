using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Application.Validators.BusinessValidators
{
    public class LibroBusinessValidator : ILibroBusinessValidator
    {
        private readonly ILibroRepository _libroRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IPrestamoRepository _prestamoRepository;

        public LibroBusinessValidator(
            ILibroRepository libroRepository,
            ICategoriaRepository categoriaRepository,
            IPrestamoRepository prestamoRepository)
        {
            _libroRepository = libroRepository;
            _categoriaRepository = categoriaRepository;
            _prestamoRepository = prestamoRepository;
        }

        public async Task<OperationResult<Categoria>> ValidateForAddAsync(AddLibroDto dto)
        {
            var existenciaResult = await _libroRepository.BuscarPorIsbnAsync(dto.ISBN);
            if (existenciaResult.IsSuccess && existenciaResult.Data is IEnumerable<Libro> lista && lista.Any())
            {
                return OperationResult<Categoria>.Failure("El ISBN proporcionado ya existe en el sistema.");
            }

            var categoriaResult = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
            if (!categoriaResult.IsSuccess || categoriaResult.Data == null)
            {
                return OperationResult<Categoria>.Failure("La categoría especificada no existe.");
            }

            return OperationResult<Categoria>.Success(categoriaResult.Data);
        }

        public async Task<OperationResult<Libro>> ValidateForUpdateAsync(int id, UpdateLibroDto dto)
        {
            var libroEntidad = await _libroRepository.ObtenerParaActualizacionAsync(id);
            if (libroEntidad == null)
            {
                return OperationResult<Libro>.Failure("Libro no encontrado.");
            }

            var categoriaResult = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
            if (!categoriaResult.IsSuccess || categoriaResult.Data == null)
            {
                return OperationResult<Libro>.Failure("La nueva categoría especificada no existe.");
            }

            return OperationResult<Libro>.Success(libroEntidad);
        }

        public async Task<OperationResult<bool>> ValidateForDeleteAsync(int id)
        {
            var prestamosActivosResult = await _prestamoRepository.FindByConditionAsync(p => p.Id == id && p.Estado == EstadoPrestamo.Activo);

            if (!prestamosActivosResult.IsSuccess)
            {
                return OperationResult<bool>.Failure(prestamosActivosResult.Message);
            }

            if (prestamosActivosResult.Data != null && prestamosActivosResult.Data.Any())
            {
                return OperationResult<bool>.Failure("No se puede eliminar el libro porque está actualmente en un préstamo activo.");
            }

            return OperationResult<bool>.Success(true);
        }
    }
}