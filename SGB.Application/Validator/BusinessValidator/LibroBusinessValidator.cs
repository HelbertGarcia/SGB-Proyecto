using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
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

        public async Task<OperationResult> ValidateForAddAsync(AddLibroDto dto)
        {
            var existenciaResult = await _libroRepository.BuscarPorIsbnAsync(dto.ISBN);
            if (existenciaResult.Success && existenciaResult.Data is IEnumerable<Libro> lista && lista.Any())
            {
                return new OperationResult { Success = false, Message = "El ISBN proporcionado ya existe en el sistema." };
            }

            var categoria = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
            if (categoria == null)
            {
                return new OperationResult { Success = false, Message = "La categoría especificada no existe." };
            }

            return new OperationResult { Success = true, Data = categoria };
        }

        public async Task<OperationResult> ValidateForUpdateAsync(int id, UpdateLibroDto dto)
        {
            var libroEntidad = await _libroRepository.ObtenerParaActualizacionAsync(id);
            if (libroEntidad == null)
            {
                return new OperationResult { Success = false, Message = "Libro no encontrado." };
            }

            var categoria = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
            if (categoria == null)
            {
                return new OperationResult { Success = false, Message = "La nueva categoría especificada no existe." };
            }

            return new OperationResult { Success = true, Data = libroEntidad };
        }

        public async Task<OperationResult> ValidateForDeleteAsync(int id)
        {
            var prestamosActivosResult = await _prestamoRepository.FindByConditionAsync(p => p.Id == id && p.Estado == EstadoPrestamo.Activo);

            if (prestamosActivosResult.Success && prestamosActivosResult.Data is IEnumerable<Prestamo> listaPrestamos && listaPrestamos.Any())
            {
                return new OperationResult { Success = false, Message = "No se puede eliminar el libro porque está actualmente en un préstamo activo." };
            }

            return new OperationResult { Success = true };
        }
    }
}