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

        public async Task<OperationResult> ValidateForAddAsync(AddCategoriaDto dto)
        {
            var resultadoExistencia = await _categoriaRepository.ObtenerPorNombreAsync(dto.Nombre);
            if (resultadoExistencia.Success && resultadoExistencia.Data != null)
            {
                return new OperationResult { Success = false, Message = "Ya existe una categoría con ese nombre." };
            }

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForDeleteAsync(int id)
        {
            var librosConCategoria = await _libroRepository.FindByConditionAsync(l => l.IDCategoria == id && l.EstaActivo);
            if (librosConCategoria.Success && librosConCategoria.Data is IEnumerable<Libro> lista && lista.Any())
            {
                return new OperationResult { Success = false, Message = "No se puede eliminar la categoría porque está asignada a uno o más libros activos." };
            }

            return new OperationResult { Success = true };
        }
    }
}