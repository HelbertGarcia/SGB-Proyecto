using Moq;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Api.Contracts.Repository.Interfaces;
using SGB.Api.Validators.BusinessValidators.Configuracion;

namespace SGB.Api.Test.ValidatorTests
{
    public class ConfiguracionValidatorTest
    {
        private readonly Mock<IConfiguracionRepository> _repoMock;
        private readonly ConfiguracionValidator _validator;

        public ConfiguracionValidatorTest()
        {
            _repoMock = new Mock<IConfiguracionRepository>();
            _validator = new ConfiguracionValidator(_repoMock.Object);
        }

        [Fact]
        public async Task ValidateForAddAsync_ShouldReturnFailure_IfConfiguracionAlreadyExists()
        {
            // Arrange
            var dto = new AddConfiguracionDto { Nombre = "Duplicado" };
            var existing = new Configuracion("Duplicado", "Valor", "Desc");
            _repoMock.Setup(r => r.ObtenerPorNombreAsync(dto.Nombre))
            .ReturnsAsync(OperationResult<Configuracion>.Success(existing));
            // Act
            var result = await _validator.ValidarAsync(dto);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Ya existe una configuración con ese nombre.", result.Message);
        }

        [Fact]
        public async Task ValidateForAddAsync_ShouldReturnSuccess_IfConfiguracionDoesNotExist()
        {
            // Arrange
            var dto = new AddConfiguracionDto { Nombre = "Nueva" };
            _repoMock.Setup(r => r.ObtenerPorNombreAsync(dto.Nombre))
            .ReturnsAsync(OperationResult<Configuracion>.Success(null));
            // Act
            var result = await _validator.ValidarAsync(dto);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnFailure_IfConfiguracionNotFound()
        {
            // Arrange
            var dto = new UpdateConfiguracionDto { IDConfiguracion = 1, Nombre = "Actualizada" };
            _repoMock.Setup(r => r.GetByIdAsync(dto.IDConfiguracion))
            .ReturnsAsync(OperationResult<Configuracion>.Failure("No existe"));
            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La configuración que desea actualizar no existe.", result.Message);
        }

        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnFailure_IfNombreIsDuplicado()
        {
            // Arrange
            var dto = new UpdateConfiguracionDto { IDConfiguracion = 1, Nombre = "Duplicado" };
            var actual = new Configuracion("Original", "val", "desc") { IDConfiguracion = 1 };
            var duplicado = new Configuracion("Duplicado", "otro", "otro") { IDConfiguracion = 2 };
            _repoMock.Setup(r => r.GetByIdAsync(dto.IDConfiguracion))
            .ReturnsAsync(OperationResult<Configuracion>.Success(actual));
            _repoMock.Setup(r => r.ObtenerPorNombreAsync(dto.Nombre))
           .ReturnsAsync(OperationResult<Configuracion>.Success(duplicado));
            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Ya existe otra configuración con ese nombre.", result.Message);
        }

        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnSuccess_IfValidUpdate()
        {
            // Arrange
            var dto = new UpdateConfiguracionDto { IDConfiguracion = 1, Nombre = "NombreActualizado" };
            var actual = new Configuracion("Original", "val", "desc") { IDConfiguracion = 1 };
            _repoMock.Setup(r => r.GetByIdAsync(dto.IDConfiguracion))
            .ReturnsAsync(OperationResult<Configuracion>.Success(actual));
            _repoMock.Setup(r => r.ObtenerPorNombreAsync(dto.Nombre))
            .ReturnsAsync(OperationResult<Configuracion>.Success(null));
            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ValidateForDeleteAsync_ShouldReturnFailure_IfNombreContainsSistemaOrProtegida()
        {
            // Arrange
            var entity = new Configuracion("ProtegidaSistema", "val", "desc") { IDConfiguracion = 1 };
            _repoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(OperationResult<Configuracion>.Success(entity));
            // Act
            var result = await _validator.ValidateForDeleteAsync(1);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Esta configuración es protegida y no puede eliminarse.", result.Message);
        }
    }
}
