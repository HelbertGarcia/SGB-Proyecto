using Moq;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using SGB.Domain.Entities.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static SGB.Api.Extensions.Loggin.LoggerExtensions;

namespace SGB.Persistence.Test
{
    public class UnitConfiguracionRepositoryTests
    {
        private readonly DbContextOptions<SGBContext> _dbOptions;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<ConfiguracionRepository>> _loggerMock;

        public UnitConfiguracionRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<SGBContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<ConfiguracionRepository>>();
        }

        private ConfiguracionRepository CreateRepository(SGBContext context)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(l => l.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);
            var appLoggerMock = new Mock<IAppLogger<ConfiguracionRepository>>().Object;
            return new ConfiguracionRepository(context, loggerFactoryMock.Object, _configMock.Object, appLoggerMock);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenConfiguracionNotFound()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);
            var result = await repo.DeleteAsync(999);
            Assert.False(result.IsSuccess);
            Assert.Equal("Configuración no encontrada.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            using var context = new SGBContext(_dbOptions);
            var config = new Configuracion("Test", "123", "desc");
            context.Configuraciones.Add(config);
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);
            var result = await repo.DeleteAsync(config.IDConfiguracion);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnFailure_WhenNombreIsEmpty()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);
            var result = await repo.ObtenerPorNombreAsync("");
            Assert.False(result.IsSuccess);
            Assert.Equal("El nombre de la configuración no puede estar vacío.", result.Message);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            using var context = new SGBContext(_dbOptions);
            var config = new Configuracion("NombreTest", "valor", "desc");
            context.Configuraciones.Add(config);
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);
            var result = await repo.ObtenerPorNombreAsync("NombreTest");
            Assert.True(result.IsSuccess);
            Assert.Equal("NombreTest", result.Data.Nombre);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_DeberiaRetornarConfiguracionCorrecta()
        {
            using var context = new SGBContext(_dbOptions);
            var nombreEsperado = "Politicas";
            context.Configuraciones.Add(new Configuracion(nombreEsperado, "Valor de prueba", "Descripción de prueba"));
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);
            var resultado = await repo.ObtenerPorNombreAsync(nombreEsperado);
            Assert.True(resultado.IsSuccess);
            Assert.Equal(nombreEsperado, resultado.Data.Nombre);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnSuccess_WhenNombreHasExtraSpaces()
        {
            using var context = new SGBContext(_dbOptions);
            var nombre = " Política Seguridad ";
            context.Configuraciones.Add(new Configuracion(nombre, "valor", "desc"));
            await context.SaveChangesAsync();
            var repo = CreateRepository(context);
            var result = await repo.ObtenerPorNombreAsync(nombre);
            Assert.True(result.IsSuccess);
            Assert.Equal(nombre, result.Data.Nombre);
        }
    }
}
