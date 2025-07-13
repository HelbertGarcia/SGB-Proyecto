using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Application.Validators.BusinessValidators
{
    public class CategoriaBusinessValidator : ICategoriaBusinessValidator
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILibroRepository _libroRepository;

        public CategoriaBusinessValidator(
            ICategoriaRepository categoriaRepository,
            ILibroRepository libroRepository)
        {
            _categoriaRepository = categoriaRepository;
            _libroRepository = libroRepository;
        }

        public async Task<OperationResult<bool>> ValidateForAddAsync(AddCategoriaDto dto)
        {
            var resultadoExistencia = await _categoriaRepository.ObtenerPorNombreAsync(dto.Nombre);

            if (resultadoExistencia.IsSuccess && resultadoExistencia.Data != null)
            {
                return OperationResult<bool>.Failure("Ya existe una categoría con ese nombre.");
            }

            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> ValidateForDeleteAsync(int id)
        {
            var librosConCategoriaResult = await _libroRepository.FindByConditionAsync(l => l.IDCategoria == id && l.EstaActivo);

            if (!librosConCategoriaResult.IsSuccess)
            {
                return OperationResult<bool>.Failure(librosConCategoriaResult.Message);
            }

            if (librosConCategoriaResult.Data != null && librosConCategoriaResult.Data.Any())
            {
                return OperationResult<bool>.Failure("No se puede eliminar la categoría porque está asignada a uno o más libros activos.");
            }

            return OperationResult<bool>.Success(true);
        }
    }
}