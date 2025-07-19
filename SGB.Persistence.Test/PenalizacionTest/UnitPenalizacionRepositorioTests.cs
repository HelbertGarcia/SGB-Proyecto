using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using Xunit;
using Moq;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Application.Loggers;
using System;
using System.Threading.Tasks;

namespace SGB.Persistence.Test
{
    public class UnitPenalizacionRepositorioTests
    {
        private readonly DbContextOptions<SGBContext> _dbOptions;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IAppLogger<PenalizacionRepository>> _appLoggerMock;

        public UnitPenalizacionRepositorioTests()
        {
        
            _dbOptions = new DbContextOptionsBuilder<SGBContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;

            _configMock = new Mock<IConfiguration>();
            _appLoggerMock = new Mock<IAppLogger<PenalizacionRepository>>();
        }


        private PenalizacionRepository CreateRepository(SGBContext context)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();

           
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
                             .Returns(Mock.Of<ILogger>());

            return new PenalizacionRepository(context, loggerFactoryMock.Object, _configMock.Object, _appLoggerMock.Object);
        }



        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenPenalizacionIsNull()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            // Act
            var result = await repo.AddAsync(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Penalizacion no puede ser nulo.", result.Message);
        }





        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenUsuarioIdInvalid()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            // Arrange: UsuarioId invalido
            var penalizacion = new Penalizacion(0, "Motivo valido", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 1, 100m);

            // Act
            var result = await repo.AddAsync(penalizacion);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("El IDUsuario es inválido.", result.Message);
        }




        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenMotivoEmpty()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            // Arrange: Motivo vacio
            var penalizacion = new Penalizacion(1, "", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 1, 100m);

            // Act
            var result = await repo.AddAsync(penalizacion);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Motivo no puede ser nulo o vacío.", result.Message);
        }




        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenPenalizacionIsValid()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            // Arrange
            var penalizacion = new Penalizacion(1, "Motivo valido", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 1, 100m);

            // Act
            var result = await repo.AddAsync(penalizacion);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(penalizacion.IDUsuario, result.Data.IDUsuario);
            Assert.Equal(penalizacion.Motivo, result.Data.Motivo);
        }



        [Fact]
        public async Task DisableAsync_ShouldReturnFailure_WhenIdInvalid()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            // Act
            var result = await repo.DisableAsync(0);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("El id es inválido.", result.Message);  

        }



        [Fact]
        public async Task DisableAsync_ShouldReturnSuccess_WhenPenalizacionIsDisabled()
        {
            using var context = new SGBContext(_dbOptions);

            // Arrange
            var penalizacion = new Penalizacion(5, "Motivo para desactivar", DateTime.UtcNow, DateTime.UtcNow.AddDays(4), 3, 90m);
            context.Penalizaciones.Add(penalizacion);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);

            // Act
            var result = await repo.DisableAsync(penalizacion.Id);

            // Assert
            Assert.True(result.IsSuccess);

            // Verificamos que la penalizacion fue desactivada en la base
            var disabledPenalizacion = await context.Penalizaciones.FindAsync(penalizacion.Id);
            Assert.False(disabledPenalizacion.EstaActivo);
        }
    }
}
