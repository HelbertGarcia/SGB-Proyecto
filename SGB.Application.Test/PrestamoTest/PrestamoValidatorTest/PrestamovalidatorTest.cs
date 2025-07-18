using Moq;
using SGB.Application.Base.ValidatorServices.Prestamos;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Application.Test.ValidatorTests
{
    public class PrestamoBusinessValidatorTest
    {
        private readonly Mock<IPrestamoRepository> _prestamoRepoMock;
        private readonly Mock<IPenalizacionRepository> _penalizacionRepoMock;
        private readonly Mock<ILibroRepository> _libroRepoMock;
        private readonly PrestamoBusinessValidator _validator;

        public PrestamoBusinessValidatorTest()
        {
            _prestamoRepoMock = new Mock<IPrestamoRepository>();
            _penalizacionRepoMock = new Mock<IPenalizacionRepository>();
            _libroRepoMock = new Mock<ILibroRepository>();
            _validator = new PrestamoBusinessValidator(
                _prestamoRepoMock.Object,
                _penalizacionRepoMock.Object,
                _libroRepoMock.Object
            );
        }

        [Fact]
        public async Task ValidateForAddAsync_ShouldReturnFailure_WhenDtoIsNull()
        {
            // Act
            var result = await _validator.ValidateForAddAsync(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Datos de préstamo inválidos.", result.Message);
        }




        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnFailure_WhenDtoIsNull()
        {
            // Act
            var result = await _validator.ValidateForUpdateAsync(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Datos de actualización inválidos.", result.Message);
        }

       
        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnSuccess_WhenValidData()
        {
            // Arrange
            var dto = new UpdatePrestamoDto { IDPrestamo = 1 };
            var prestamo = new Prestamo(1, "1234567890123", DateTime.Now, DateTime.Now.AddDays(7));

            _prestamoRepoMock.Setup(r => r.GetByIdAsync(dto.IDPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));

            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Validación exitosa.", result.Data);
        }

        [Fact]
        public async Task ValidateForDisableAsync_ShouldReturnFailure_WhenDtoIsNull()
        {
            // Act
            var result = await _validator.ValidateForDisableAsync(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Datos inválidos para deshabilitar préstamo.", result.Message);
        }

        [Fact]
        public async Task ValidateForDisableAsync_ShouldReturnFailure_WhenPrestamoNotFound()
        {
            // Arrange
            var dto = new DiseblePrestamoDto { IDPrestamo = 999 };

            _prestamoRepoMock.Setup(r => r.GetByIdAsync(dto.IDPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Failure("No encontrado"));

            // Act
            var result = await _validator.ValidateForDisableAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Préstamo no encontrado.", result.Message);
        }

    }
}