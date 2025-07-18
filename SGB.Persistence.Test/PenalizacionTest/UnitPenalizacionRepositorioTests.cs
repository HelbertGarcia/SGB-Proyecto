using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using Xunit;
using Moq;

using SGB.Domain.Entities.Penalizaciones;


namespace SGB.Persistence.Test
{
    public class UnitPenalizacionRepositorioTests
    {
        private readonly DbContextOptions<SGBContext> _dbOptions;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<PenalizacionRepository>> _loggerMock;


        public UnitPenalizacionRepositorioTests()
        {
            _dbOptions = new DbContextOptionsBuilder<SGBContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;

            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<PenalizacionRepository>>();

        }

        private PenalizacionRepository CreateRepository(SGBContext context)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
                             .Returns(_loggerMock.Object);

            return new PenalizacionRepository(context, loggerFactoryMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenPenalizacionIsNull()
        {

            
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            
            var result = await repo.AddAsync(null);

            Assert.False(result.IsSuccess);
            Assert.Equal("La penalización no puede ser nula.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenUsuarioIdInvalid()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            var penalizacion = new Penalizacion(0, "Motivo valido", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 1, 100m);

            var result = await repo.AddAsync(penalizacion);

            Assert.False(result.IsSuccess);
            Assert.Equal("El ID de usuario es inválido.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenMotivoEmpty()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            var penalizacion = new Penalizacion(1, "", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 1, 100m);

            var result = await repo.AddAsync(penalizacion);

            Assert.False(result.IsSuccess);
            Assert.Equal("El motivo de la penalización no puede ser nulo o vacío.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenPenalizacionIsValid()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            var penalizacion = new Penalizacion(1, "Motivo valido", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 1, 100m);

            var result = await repo.AddAsync(penalizacion);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(penalizacion.IDUsuario, result.Data.IDUsuario);
        }

      

       

        [Fact]
        public async Task DisableAsync_ShouldReturnFailure_WhenIdInvalid()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            var result = await repo.DisableAsync(0);

            Assert.False(result.IsSuccess);
            Assert.Equal("ID inválido para desactivar penalización.", result.Message);
        }

        [Fact]
        public async Task DisableAsync_ShouldReturnSuccess_WhenPenalizacionIsDisabled()
        {
            using var context = new SGBContext(_dbOptions);

            var penalizacion = new Penalizacion(5, "Motivo para desactivar", DateTime.UtcNow, DateTime.UtcNow.AddDays(4), 3, 90m);
            context.Penalizaciones.Add(penalizacion);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);

            var result = await repo.DisableAsync(penalizacion.Id);

            Assert.True(result.IsSuccess);

            var disabledPenalizacion = await context.Penalizaciones.FindAsync(penalizacion.Id);
            Assert.False(disabledPenalizacion.EstaActivo);
        }

      
       






    }
}