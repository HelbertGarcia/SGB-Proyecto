using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SGB.Application.Services.LibrosServices;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Base;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using SGB.Domain.Entities.Libro;
using SGB.Application.Validators.BusinessValidators;

namespace SGB.Application.Test
{
    public class CategoriaServiceTests
    {
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepo;
        private readonly Mock<ILibroRepository> _mockLibroRepo;
        private readonly Mock<ICategoriaBusinessValidator> _mockCategoriaValidator;
        private readonly ILogger<CategoriaService> _mockLogger;
        private readonly IConfiguration _configuration;

        public CategoriaServiceTests()
        {
            _mockCategoriaRepo = new Mock<ICategoriaRepository>();
            _mockLibroRepo = new Mock<ILibroRepository>();
            _mockCategoriaValidator = new Mock<ICategoriaBusinessValidator>();
            _mockLogger = new Mock<ILogger<CategoriaService>>().Object;
            _configuration = new ConfigurationBuilder().Build();
        }

        private CategoriaService CreateService()
        {
            return new CategoriaService(
                _mockCategoriaRepo.Object,
                _mockCategoriaValidator.Object, 
                _mockLogger,
                _configuration
            );
        }

        [Fact]
        public async Task GetByIdAsync_DebeDevolverUnDto_CuandoLaCategoriaExiste()
        {
            var categoriaId = 1;
            var nombreCategoria = "Novela";

            var categoriaDePrueba = new Categoria(nombreCategoria);

            _mockCategoriaRepo.Setup(r => r.GetByIdAsync(categoriaId))
                .ReturnsAsync(OperationResult<Categoria>.Success(categoriaDePrueba));

            var service = CreateService();

            // Act (Actuar)
            var resultado = await service.GetByIdAsync(categoriaId);

            // Assert (Afirmar)
            resultado.IsSuccess.ShouldBeTrue();
            resultado.Data.ShouldNotBeNull();

            resultado.Data.Nombre.ShouldBe(nombreCategoria);
        }

        [Fact]
        public async Task AddAsync_DebeFallar_SiElValidadorDeNegocioFalla()
        {
            // Arrange
            var dto = new AddCategoriaDto { Nombre = "Existente" };

            _mockCategoriaValidator.Setup(v => v.ValidateForAddAsync(dto))
                .ReturnsAsync(OperationResult<bool>.Failure("Ya existe una categoría con ese nombre."));

            var service = CreateService();

            // Act
            var resultado = await service.AddAsync(dto);

            // Assert
            resultado.IsSuccess.ShouldBeFalse();
            resultado.Message.ShouldBe("Ya existe una categoría con ese nombre.");
        }

        [Fact]
        public async Task DeleteAsync_DebeFallar_SiElValidadorDetectaQueLaCategoriaEstaEnUso()
        {
            // Arrange
            var categoriaId = 1;

            _mockCategoriaValidator.Setup(v => v.ValidateForDeleteAsync(categoriaId))
                .ReturnsAsync(OperationResult<bool>.Failure("La categoría está en uso."));

            var service = CreateService();

            // Act
            var resultado = await service.DeleteAsync(categoriaId);

            // Assert
            resultado.IsSuccess.ShouldBeFalse();
            resultado.Message.ShouldBe("La categoría está en uso.");
        }
    }
}