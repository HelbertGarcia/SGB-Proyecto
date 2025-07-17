using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using Xunit;

namespace SGB.Application.Test
{
    public class UnitConfiguracionServiceTest
    {
        private readonly Mock<IConfiguracionRepository> _repoMock;
        private readonly Mock<ILogger<ConfiguracionService>> _loggerMock;
        private readonly ConfiguracionService _service;

        public UnitConfiguracionServiceTest()
        {
            _repoMock = new Mock<IConfiguracionRepository>();
            _loggerMock = new Mock<ILogger<ConfiguracionService>>();
            _service = new ConfiguracionService(_repoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenNombreOrValorIsEmpty()
        {
            // Arrange
            var dto = new AddConfiguracionDto { Nombre = "", Valor = "valor" };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("El nombre y el valor son obligatorios.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenInputIsValid()
        {
            // Arrange
            var dto = new AddConfiguracionDto { Nombre = "Nombre", Valor = "Valor", Descripcion = "Desc" };
            var entity = new Configuracion("Nombre", "Valor", "Desc");

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Configuracion>()))
                     .ReturnsAsync(OperationResult<Configuracion>.Success(entity));

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Configuración guardada correctamente.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenConfiguracionNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(OperationResult<Configuracion>.Failure("Configuración no encontrada."));

            // Act
            var result = await _service.UpdateAsync(99, new UpdateConfiguracionDto());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Configuración no encontrada.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _service.DeleteAsync(invalidId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ID inválido para eliminación.", result.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var configuraciones = new List<Configuracion>
            {
                new Configuracion("Conf1", "Val1", "Desc1"),
                new Configuracion("Conf2", "Val2", "Desc2")
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(OperationResult<IEnumerable<Configuracion>>.Success(configuraciones));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            // Arrange
            var config = new Configuracion("Test", "123", "desc");
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(OperationResult<Configuracion>.Success(config));

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Test", result.Data.Nombre);
        }
    }
}
