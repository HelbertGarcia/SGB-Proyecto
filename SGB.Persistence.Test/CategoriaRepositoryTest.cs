using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Entities.Categoria;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace SGB.Persistence.Test
{
    public class CategoriaRepositoryTests
    {
        private readonly DbContextOptions<SGBContext> _dbContextOptions;
        private readonly SGBContext _context;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _configuration;

        public CategoriaRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "SGB_TestDB_" + System.Guid.NewGuid())
                .Options;

            _context = new SGBContext(_dbContextOptions);
            _loggerFactory = new LoggerFactory();
            _configuration = new ConfigurationBuilder().Build();

            _context.Categorias.Add(new Categoria("Novela"));
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByIdAsync_DebeDevolverLaCategoriaCorrecta_CuandoIdExiste()
        {
            // Arrange
            var repo = new CategoriaRepository(_context, _loggerFactory, _configuration);

            // Act
            var resultado = await repo.GetByIdAsync(1); 

            // Assert
            resultado.IsSuccess.ShouldBeTrue();
            resultado.Data.ShouldNotBeNull();
            resultado.Data.Nombre.ShouldBe("Novela");
        }

        [Fact]
        public async Task AddAsync_DebeGuardarUnaNuevaCategoria_Correctamente()
        {
            // Arrange
            var repo = new CategoriaRepository(_context, _loggerFactory, _configuration);
            var nuevaCategoria = new Categoria("Ciencia Ficción");

            // Act
            await repo.AddAsync(nuevaCategoria);

            // Assert
            var categoriaGuardada = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Ciencia Ficción");
            categoriaGuardada.ShouldNotBeNull();
        }
    }
}