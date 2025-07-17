using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;

namespace SGB.Persistence.Test
{
    public class UnitConfiguracionRepositoryTests
    {
        private readonly DbContextOptions<SGBContext> _dbOptions;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<ConfiguracionRepository>> _loggerMock;

        public UnitConfiguracionRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<ConfiguracionRepository>>();
        }

        private ConfiguracionRepository CreateRepository(SGBContext context)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(l => l.CreateLogger(It.IsAny<string>()))
                             .Returns(_loggerMock.Object);

            return new ConfiguracionRepository(context, loggerFactoryMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenConfiguracionNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "Delete_Fail_DB")
                .Options;

            using var context = new SGBContext(options);
            var repo = CreateRepository(context);

            // Act
            var result = await repo.DeleteAsync(99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Configuración no encontrada.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "Delete_Success_DB")
                .Options;

            using var context = new SGBContext(options);
            var config = new Configuracion("Test", "123", "desc");
            context.Configuraciones.Add(config);
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);

            // Act
            var result = await repo.DeleteAsync(config.IDConfiguracion);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnFailure_WhenNombreIsEmpty()
        {
            // Arrange
            var repo = CreateRepository(new Mock<SGBContext>().Object);

            // Act
            var result = await repo.ObtenerPorNombreAsync("");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("El nombre de la configuración no puede estar vacío.", result.Message);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "GetByName_DB")
                .Options;

            using var context = new SGBContext(options);
            var config = new Configuracion("NombreTest", "valor", "desc");
            context.Configuraciones.Add(config);
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);

            // Act
            var result = await repo.ObtenerPorNombreAsync("NombreTest");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("NombreTest", result.Data.Nombre);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_ShouldReturnFailure_WhenNotFound()
        {
            // Arrange
            var repo = CreateRepository(new Mock<SGBContext>().Object);

            // Act
            var result = await repo.ObtenerPorIdAsync(999);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Configuración no encontrada.", result.Message);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_ShouldReturnSuccess_WhenFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "GetById_DB")
                .Options;

            using var context = new SGBContext(options);
            var config = new Configuracion("IdTest", "val", "desc");
            context.Configuraciones.Add(config);
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);

            // Act
            var result = await repo.ObtenerPorIdAsync(config.IDConfiguracion);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("IdTest", result.Data.Nombre);
        }
    }
}
