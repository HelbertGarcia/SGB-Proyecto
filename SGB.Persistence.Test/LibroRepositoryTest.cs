using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SGB.Persistence.Test
{
    public class LibroRepositoryTest
    {
        private readonly DbContextOptions<SGBContext> _dbContextOptions;
        private readonly SGBContext _context;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _configuration;

        public LibroRepositoryTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "SGB_TestDB_" + System.Guid.NewGuid())
                .Options;

            _context = new SGBContext(_dbContextOptions);
            _loggerFactory = new LoggerFactory();
            _configuration = new ConfigurationBuilder().Build(); 

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var categorias = new List<Categoria>
            {
                new Categoria("Ciencia Ficción"),
                new Categoria("Novela")
            };
            _context.Categorias.AddRange(categorias);

            var libros = new List<Libro>
            {
                new Libro("bbbbbbbbbb", "Dune", "Frank Herbert", "Chilton Books", new System.DateTime(1965, 8, 1), 1),
                new Libro("ppppppppppppp", "Cien Años de Soledad", "Gabriel García Márquez", "Sudamericana", new System.DateTime(1967, 5, 30), 2)
            };
            _context.Libros.AddRange(libros);

            _context.SaveChanges();
        }


        [Fact]
        public async Task GetByIdAsync_DebeDevolverElLibroCorrecto_CuandoElIdExiste()
        {
            var repo = new LibroRepository(_context, _loggerFactory, _configuration);
            var libroExistente = await _context.Libros.FirstAsync();

            var resultado = await repo.GetByIdAsync(libroExistente.Id);

            resultado.ShouldNotBeNull();
            resultado.IsSuccess.ShouldBeTrue();
            resultado.Data.Id.ShouldBe(libroExistente.Id);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarUnNuevoLibro_ALaBaseDeDatos()
        {
            var repo = new LibroRepository(_context, _loggerFactory, _configuration);
            var nuevoLibro = new Libro("bbbbbbbbhhjjk", "El Hobbit", "J.R.R. Tolkien", "Allen & Unwin", null, 1);

            // Act
            await repo.AddAsync(nuevoLibro);

            // Assert

            using (var assertContext = new SGBContext(_dbContextOptions))
            {
                var libroAgregado = await assertContext.Libros.FirstOrDefaultAsync(l => l.ISBN == "bbbbbbbbhhjjk");
                libroAgregado.ShouldNotBeNull();
                libroAgregado.Titulo.ShouldBe("El Hobbit");
            }
        }

        [Fact]
        public async Task DeleteAsync_DebeRealizarUnBorradoLogico_CambiandoElEstado()
        {
            var repo = new LibroRepository(_context, _loggerFactory, _configuration);
            var libroExistente = await _context.Libros.FirstAsync();

            await repo.DeleteAsync(libroExistente.Id);

            var libroDespuesDeBorrar = await _context.Libros.IgnoreQueryFilters().FirstOrDefaultAsync(l => l.Id == libroExistente.Id);
            libroDespuesDeBorrar.ShouldNotBeNull();
            libroDespuesDeBorrar.EstaActivo.ShouldBeFalse();
        }

        [Fact]
        public async Task BuscarPorAutorAsync_DebeDevolverSoloLosLibrosDeEseAutor()
        {
            // Arrange
            var repo = new LibroRepository(_context, _loggerFactory, _configuration);

            // Act
            var resultado = await repo.BuscarPorAutorAsync("Frank Herbert");

            // Assert
            resultado.IsSuccess.ShouldBeTrue();
            resultado.Data.ShouldHaveSingleItem(); 
            resultado.Data.First().Autor.ShouldBe("Frank Herbert");
        }

        [Fact]
        public async Task ObtenerParaActualizacionAsync_DebeDevolverEntidadRastreable()
        {
            var repo = new LibroRepository(_context, _loggerFactory, _configuration);
            var libroExistente = await _context.Libros.AsNoTracking().FirstAsync();

            var libroRastreado = await repo.ObtenerParaActualizacionAsync(libroExistente.Id);

            libroRastreado.ActualizarDetalles(
                "Título Cambiado",          
                libroRastreado.Autor,       
                libroRastreado.Editorial,    
                libroRastreado.FechaPublicacion,
                libroRastreado.IDCategoria 
            );

            await _context.SaveChangesAsync();

            var libroActualizado = await _context.Libros.FindAsync(libroExistente.Id);
            libroActualizado.ShouldNotBeNull();
            libroActualizado.Titulo.ShouldBe("Título Cambiado");
        }

        [Fact]
        public async Task ObtenerDetallesDTOPorIdAsync_DebeDevolverUnDtoConElNombreDeLaCategoria()
        {
            var repo = new LibroRepository(_context, _loggerFactory, _configuration);
            var libroExistente = await _context.Libros.FirstAsync(l => l.Autor == "Frank Herbert");

            var resultado = await repo.ObtenerDetallesDTOPorIdAsync(libroExistente.Id);

            resultado.IsSuccess.ShouldBeTrue();
            resultado.Data.ShouldNotBeNull();
            resultado.Data.NombreCategoria.ShouldBe("Ciencia Ficción");
        }
    }
}