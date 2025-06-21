using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<LibroRepository> _logger;
        private readonly IConfiguration _configuration;

        public LibroRepository(SGBContext context,
                               ILogger<LibroRepository> logger,
                               IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        // aqui estan todos los metodos que se heredan de BaseRepository<T>

        public override Task<OperationResult> AddAsync(Libro entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<OperationResult> DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }

        public override Task<IEnumerable<Libro>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<OperationResult> FindByConditionAsync(Expression<Func<Libro, bool>> filter)
        {
            return base.FindByConditionAsync(filter);
        }

        public override Task<Libro> GetByIdAsync(int id)
        {
            return base.GetByIdAsync(id);
        }

        public override Task<OperationResult> UpdateAsync(Libro entity)
        {
            return base.UpdateAsync(entity);
        }

        //aqui van los metodos propios de LibroRepository

        public async Task<OperationResult> BuscarPorAutor(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
            {
                return new OperationResult { Success = false, Message = "El autor no puede estar vacío." };
            }

            try
            {
                var data = await Entity.Where(l => l.Autor.Contains(autor) && l.EstaActivo)
                                       .AsNoTracking()
                                       .ToListAsync();

                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al buscar libros por autor: {Autor}", autor);
                return new OperationResult { Success = false, Message = "Ocurrió un error inesperado al buscar por autor." };
            }
        }

        public Task<OperationResult> BuscarPorTitulo(string titulo)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> BuscarPorEditorial(string editorial)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> BuscarPorIsbnAsync(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> BuscarPorCategoriaAsync(string nombreCategoria)
        {
            throw new NotImplementedException();
        }

    }
}
