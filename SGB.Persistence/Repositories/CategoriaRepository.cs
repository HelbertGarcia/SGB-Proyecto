using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<CategoriaRepository> _logger; 
        private readonly IConfiguration _configuration;

        public CategoriaRepository(SGBContext context,
                                   ILoggerFactory loggerFactory,
                                   IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<CategoriaRepository>();
        }

        public async Task<OperationResult> ObtenerPorNombreAsync(string nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(nombreCategoria))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El nombre de la categoría no puede estar vacío." });
            }

            try
            {
                var categoria = await Entity
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(c => c.Nombre == nombreCategoria);

                return new OperationResult { Data = categoria };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Categorias:GetByNameError"] ?? "Ocurrió un error al buscar la categoría por nombre.";
                _logger.LogError(ex, "{ErrorMessage} para el nombre: {NombreCategoria}", errorMessage, nombreCategoria);

                return new OperationResult { Success = false, Message = errorMessage };
            }
        }
    }
}