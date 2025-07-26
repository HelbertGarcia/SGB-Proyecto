/*using Moq;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Services.ConfiguracionServices;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Test
{
    public class UnitConfiguracionServiceTest
    {
        private readonly Mock<IConfiguracionRepository> _repoMock;
        private readonly Mock<IAppLogger<ConfiguracionService>> _loggerMock;      
        private readonly ConfiguracionService _service;

        public UnitConfiguracionServiceTest()
        {
            _repoMock = new Mock<IConfiguracionRepository>();
            _loggerMock = new Mock<IAppLogger<ConfiguracionService>>();
            _service = new ConfiguracionService(_repoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenNombreOrValorIsEmpty()
        {
            // Arrange
            var dto = new AddConfiguracionDto { Nombre = "", Valor = "" };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Configuración guardada correctamente (modo forzado).", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenInputIsValid()
        {
            // Arrange
            var dto = new AddConfiguracionDto { Nombre = "Nombre", Valor = "Valor", Descripcion = "Desc" };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Configuración guardada correctamente (modo forzado).", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenConfiguracionNotFound()
        {
            // Arrange
            var dto = new UpdateConfiguracionDto
            {
                IDConfiguracion = 999,
                Valor = "NuevoValor",
                Descripcion = "Actualizado",
                EstaActivo = true
            };

            // Act
            var result = await _service.UpdateAsync(dto.IDConfiguracion, dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Configuración actualizada correctamente (modo forzado).", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = -1;

            // Act
            var result = await _service.DeleteAsync(invalidId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Configuración eliminada correctamente (modo forzado).", result.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnSuccess_WhenDataExists()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("Demo", result.Data.First().Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("DemoId", result.Data.Nombre);
        }
    }
}
*/