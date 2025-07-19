using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using global::SGB.Persistence.Context;
using global::SGB.Persistence.Repositories;
using global::SGB.Domain.Entities.Prestamos;
using SGB.Application.Loggers;

namespace SGB.Persistence.Test.PrestamoTests
{
    public class UnitTestPrestamoRepositoryTests
    {
        private readonly DbContextOptions<SGBContext> _dbOptions;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IAppLogger<PrestamoRepository>> _appLoggerMock;

        public UnitTestPrestamoRepositoryTests()
        {
           
            _dbOptions = new DbContextOptionsBuilder<SGBContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;

            _configMock = new Mock<IConfiguration>();
            _appLoggerMock = new Mock<IAppLogger<PrestamoRepository>>();
        }

        private PrestamoRepository CreateRepository(SGBContext context)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();

           
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
                             .Returns(Mock.Of<ILogger>());

            return new PrestamoRepository(context, loggerFactoryMock.Object, _configMock.Object, _appLoggerMock.Object);
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
            Assert.Equal("Prestamo no puede ser nulo.", result.Message);
        }




        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenPrestamoIsValid()
        {
            using var context = new SGBContext(_dbOptions);

            // Arrange
            var prestamo = new Prestamo(
                usuarioId: 1,
                isbn: "1234567890123",
                fechaInicio: DateTime.UtcNow,
                fechaFin: DateTime.UtcNow.AddDays(7)
            );

            var repo = CreateRepository(context);

            // Act
            var result = await repo.AddAsync(prestamo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(prestamo.UsuarioId, result.Data.UsuarioId);
            Assert.Equal(prestamo.ISBN, result.Data.ISBN);
        }




        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenPrestamoIsValid()
        {
            using var context = new SGBContext(_dbOptions);

            // Arrange: agregamos un prestamo inicial
            var prestamo = new Prestamo(
                usuarioId: 4,
                fechaInicio: DateTime.UtcNow,
                fechaFin: DateTime.UtcNow.AddDays(10),
                isbn: "1234567890123"
            );

            context.Prestamos.Add(prestamo);
            await context.SaveChangesAsync();

            // Modificamos el prestamo
            prestamo.RegistrarDevolucion(DateTime.UtcNow.AddDays(5));
            var repo = CreateRepository(context);

            // Act
            var result = await repo.UpdateAsync(prestamo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(prestamo.FechaDevolucion, result.Data.FechaDevolucion);
        }




        [Fact]
        public async Task DisableAsync_ShouldReturnFailure_WhenIdIsInvalid()
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
        public async Task DisableAsync_ShouldReturnSuccess_WhenPrestamoIsDisabled()
        {

            using var context = new SGBContext(_dbOptions);

            // Arrange
            var prestamo = new Prestamo(
                usuarioId: 5,
                fechaInicio: DateTime.UtcNow,
                fechaFin: DateTime.UtcNow.AddDays(4),
                isbn: "1234567890123"
            );

            context.Prestamos.Add(prestamo);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);

            // Act
            var result = await repo.DisableAsync(prestamo.Id);

            // Assert
            Assert.True(result.IsSuccess);

            // Verificamos que el préstamo fue desactivado en la base
            var disabledPrestamo = await context.Prestamos.FindAsync(prestamo.Id);
            Assert.False(disabledPrestamo.EstaActivo);
        }
    }
}
