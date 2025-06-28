using SGB.Application.Contratos.interfaces.LibrosServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contratos.Servicios.Libros
{
    public class LibroService : ILibroService
    {
        private readonly LibroRepository _libroRepository;

        public Task<OperationResult> AddAsync<T>(T entity) where T : class
        {
            _libroRepository.AddAsync();
        }

        public Task<OperationResult> DeleteAsync<T>(int id) where T : class
        {

        }

        public Task<T> GetByIdAsync<T>(T id) where T : class
        {

        }

        public Task<OperationResult> UpdateAsync<T>(T entity) where T : class
        {

        }
    }
}
