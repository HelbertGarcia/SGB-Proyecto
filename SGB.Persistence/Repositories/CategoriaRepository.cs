using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly ILogger<CategoriaRepository> _logger;
        private readonly IConfiguration _configuration;

        public CategoriaRepository(SGBContext context,
                                   ILoggerFactory loggerFactory,
                                   IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<CategoriaRepository>();
        }

        public async Task<OperationResult<Categoria>> ObtenerPorNombreAsync(string nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(nombreCategoria))
            {
                return OperationResult<Categoria>.Failure("El nombre de la categoría no puede estar vacío.");
            }

            try
            {
                var categoria = await Entity
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(c => c.Nombre == nombreCategoria);

                return OperationResult<Categoria>.Success(categoria);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Categorias:GetByNameError"] ?? "Ocurrió un error al buscar la categoría por nombre.";
                _logger.LogError(ex, "{ErrorMessage} para el nombre: {NombreCategoria}", errorMessage, nombreCategoria);

                return OperationResult<Categoria>.Failure(errorMessage);
            }
        }
    }
}