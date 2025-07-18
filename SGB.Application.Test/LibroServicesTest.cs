using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SGB.Application.Services.LibrosServices;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Base;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Application.Test
{
    public class LibroServicesTest
    {
        private readonly Mock<ILibroRepository> _mockLibroRepo;
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepo;
        private readonly Mock<IPrestamoRepository> _mockPrestamoRepo;
        private readonly ILogger<LibroService> _mockLogger;
        private readonly IConfiguration _configuration;

        public LibroServicesTest()
        {
            _mockLibroRepo = new Mock<ILibroRepository>();
            _mockCategoriaRepo = new Mock<ICategoriaRepository>();
            _mockPrestamoRepo = new Mock<IPrestamoRepository>();
            _mockLogger = new Mock<ILogger<LibroService>>().Object;

            var inMemorySettings = new Dictionary<string, string> {
                {"ErrorMessages:Global:ResourceNotFound", "Recurso no encontrado."},
                {"ErrorMessages:Libros:IsbnAlreadyExists", "El ISBN proporcionado ya existe en el sistema."},
                {"ErrorMessages:Libros:BookIsOnLoan", "No se puede eliminar el libro porque está actualmente en un préstamo activo."}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private LibroService CreateService()
        {
            return new LibroService(
                _mockLibroRepo.Object,
                _mockPrestamoRepo.Object,
                _mockCategoriaRepo.Object,
                _mockLogger,
                _configuration
            );
        }

        [Fact]
        public async Task AddAsync_DebeFallar_SiElIsbnYaExiste()
        {
            // Arrange
            var addDto = new AddLibroDto { ISBN = "1234567890" };

            var libroExistente = new Libro("1234567890", "Otro Título", "Otro Autor", null, null, 1);
            _mockLibroRepo.Setup(r => r.BuscarPorIsbnAsync(addDto.ISBN))
                .ReturnsAsync(OperationResult<Libro>.Success(libroExistente));

            var service = CreateService();

            // Act
            var resultado = await service.AddAsync(addDto);

            // Assert
            resultado.IsSuccess.ShouldBeFalse();
            resultado.Message.ShouldBe("El ISBN proporcionado ya existe en el sistema.");
        }

        [Fact]
        public async Task UpdateAsync_DebeFallar_SiElLibroNoExiste()
        {
            // Arrange
            var libroIdQueNoExiste = 99;
            var updateDto = new UpdateLibroDto { /* datos */ };

            _mockLibroRepo.Setup(r => r.ObtenerParaActualizacionAsync(libroIdQueNoExiste))
                .ReturnsAsync((Libro)null);

            var service = CreateService();

            // Act
            var resultado = await service.UpdateAsync(libroIdQueNoExiste, updateDto);

            // Assert
            resultado.IsSuccess.ShouldBeFalse();
            resultado.Message.ShouldBe("Recurso no encontrado.");
        }

        [Fact]
        public async Task DeleteAsync_DebeFallar_SiElLibroEstaPrestado()
        {
            var libroIdParaEliminar = 1;
            var isbnDelLibro = "978-VALIDO"; 

            var libroExistente = new Libro(isbnDelLibro, "Título de Prueba", "Autor", "Ed", null, 1);
            _mockLibroRepo.Setup(r => r.GetByIdAsync(libroIdParaEliminar))
                           .ReturnsAsync(OperationResult<Libro>.Success(libroExistente));

            var prestamos = new List<Prestamo> { new Prestamo(isbnDelLibro, 1, 15) };
            _mockPrestamoRepo.Setup(r => r.FindByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Prestamo, bool>>>()))
                .ReturnsAsync(OperationResult<IEnumerable<Prestamo>>.Success(prestamos));

            var service = CreateService();

            // Act (Actuar)
            var resultado = await service.DeleteAsync(libroIdParaEliminar);

            // Assert (Afirmar)
            resultado.IsSuccess.ShouldBeFalse();
            resultado.Message.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllAsync_DebeDevolverUnaListaDeDtos()
        {
            // Arrange
            var listaDtos = new List<LibroDto> { new LibroDto(), new LibroDto() };
            _mockLibroRepo.Setup(r => r.ObtenerTodosConDetallesAsync())
                .ReturnsAsync(OperationResult<IEnumerable<LibroDto>>.Success(listaDtos));

            var service = CreateService();

            // Act
            var resultado = await service.GetAllAsync();

            // Assert
            resultado.IsSuccess.ShouldBeTrue();
            ((List<LibroDto>)resultado.Data).Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetByIdAsync_DebeLlamarAlRepositorioYDevolverDto()
        {
            // Arrange
            var libroId = 1;
            var libroDto = new LibroDto { Id = libroId, Titulo = "Libro Encontrado" };
            _mockLibroRepo.Setup(r => r.ObtenerDetallesDTOPorIdAsync(libroId))
                .ReturnsAsync(OperationResult<LibroDto>.Success(libroDto));

            var service = CreateService();

            // Act
            var resultado = await service.GetByIdAsync(libroId);

            // Assert
            resultado.IsSuccess.ShouldBeTrue();
            resultado.Data.Titulo.ShouldBe("Libro Encontrado");
        }


    }
}