using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SGB.Application.Base.ValidatorServices.Prestamos;
using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Application.Loggers;
using SGB.Application.Services.Prestamos_y_PenalizacionServices;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Application.Test.ServiceTests
{
    public class PrestamoServiceTest
    {
        private readonly Mock<IPrestamoRepository> _repoMock;
        private readonly Mock<IAppLogger<PrestamoService>> _loggerMock; // CAMBIO aquí
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IValidator<AddPrestamoDto>> _addValidatorMock;
        private readonly Mock<IValidator<UpdatePrestamoDto>> _updateValidatorMock;
        private readonly Mock<IValidator<DiseblePrestamoDto>> _disableValidatorMock;
        private readonly Mock<IPrestamoBusinessValidator> _businessValidatorMock;
        private readonly Mock<IPrestamoMapper> _mapperMock;
        private readonly Mock<IPenalizacionServices> _penalizacionServiceMock;
        private readonly PrestamoService _service;

        public PrestamoServiceTest()
        {
            _repoMock = new Mock<IPrestamoRepository>();
            _loggerMock = new Mock<IAppLogger<PrestamoService>>(); // CAMBIO
            _configurationMock = new Mock<IConfiguration>();
            _addValidatorMock = new Mock<IValidator<AddPrestamoDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdatePrestamoDto>>();
            _disableValidatorMock = new Mock<IValidator<DiseblePrestamoDto>>();
            _businessValidatorMock = new Mock<IPrestamoBusinessValidator>();
            _mapperMock = new Mock<IPrestamoMapper>();
            _penalizacionServiceMock = new Mock<IPenalizacionServices>();

            _service = new PrestamoService(
                _repoMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _addValidatorMock.Object,
                _updateValidatorMock.Object,
                _disableValidatorMock.Object,
                _businessValidatorMock.Object,
                _mapperMock.Object,
                _penalizacionServiceMock.Object
            );
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenFluentValidationFails()
        {
            // Arrange
            var dto = new AddPrestamoDto { UsuarioId = 1, ISBN = "1234567890123" };
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("ISBN", "ISBN es requerido"));

            _addValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ISBN es requerido", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenBusinessValidationFails()
        {
            // Arrange
            var dto = new AddPrestamoDto { UsuarioId = 1, ISBN = "1234567890123" };
            var validationResult = new ValidationResult();

            _addValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);
            _businessValidatorMock.Setup(b => b.ValidateForAddAsync(dto))
                .ReturnsAsync(OperationResult<string>.Failure("El usuario tiene penalizaciones activas."));

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("El usuario tiene penalizaciones activas.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenValidInput()
        {
            // Arrange
            var dto = new AddPrestamoDto { UsuarioId = 1, ISBN = "1234567890123" };
            var prestamo = new Prestamo(1, "1234567890123", DateTime.Now, DateTime.Now.AddDays(7));
            var responseDto = new PrestamoResponseDto { Id = 1, ISBN = "1234567890123" };
            var validationResult = new ValidationResult();

            _addValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);
            _businessValidatorMock.Setup(b => b.ValidateForAddAsync(dto))
                .ReturnsAsync(OperationResult<string>.Success("Validación exitosa"));
            _mapperMock.Setup(m => m.MapFromAddDto(dto)).Returns(prestamo);
            _repoMock.Setup(r => r.AddAsync(prestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));
            _mapperMock.Setup(m => m.MapToDto(prestamo)).Returns(responseDto);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Préstamo registrado correctamente.", result.Message);
            Assert.Equal(responseDto, result.Data);
        }

       
        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenFluentValidationFails()
        {
            // Arrange
            var dto = new DiseblePrestamoDto { IDPrestamo = 0 };
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("IDPrestamo", "ID inválido"));

            _disableValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _service.DeleteAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ID inválido", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenPrestamoNotFound()
        {
            // Arrange
            var dto = new DiseblePrestamoDto { IDPrestamo = 999 };
            var validationResult = new ValidationResult();

            _disableValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);
            _businessValidatorMock.Setup(b => b.ValidateForDisableAsync(dto))
                .ReturnsAsync(OperationResult<string>.Success("Validación exitosa"));
            _repoMock.Setup(r => r.GetByIdAsync(dto.IDPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Failure("No encontrado"));

            // Act
            var result = await _service.DeleteAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Préstamo no encontrado.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenValidDelete()
        {
            // Arrange
            var dto = new DiseblePrestamoDto { IDPrestamo = 1 };
            var prestamo = new Prestamo(1, "1234567890123", DateTime.Now, DateTime.Now.AddDays(7));
            var validationResult = new ValidationResult();

            _disableValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);
            _businessValidatorMock.Setup(b => b.ValidateForDisableAsync(dto))
                .ReturnsAsync(OperationResult<string>.Success("Validación exitosa"));
            _repoMock.Setup(r => r.GetByIdAsync(dto.IDPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));
            _repoMock.Setup(r => r.UpdateAsync(prestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));

            // Act
            var result = await _service.DeleteAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Préstamo desactivado correctamente.", result.Message);
        }

        [Fact]
        public async Task RegistrarDevolucionAsync_ShouldReturnFailure_WhenPrestamoNotFound()
        {
            // Arrange
            var dto = new RegistrarDevolucionDto { IdPrestamo = 999, FechaDevolucion = DateTime.Now };

            _repoMock.Setup(r => r.GetByIdAsync(dto.IdPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Failure("No encontrado"));

            // Act
            var result = await _service.RegistrarDevolucionAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Préstamo no encontrado.", result.Message);
        }

        [Fact]
        public async Task RegistrarDevolucionAsync_ShouldReturnFailure_WhenAlreadyReturned()
        {
            // Arrange
            var dto = new RegistrarDevolucionDto { IdPrestamo = 1, FechaDevolucion = DateTime.Now };
            var prestamo = new Prestamo(1, "1234567890123", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-3));
            prestamo.RegistrarDevolucion(DateTime.Now.AddDays(-2));

            _repoMock.Setup(r => r.GetByIdAsync(dto.IdPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));

            // Act
            var result = await _service.RegistrarDevolucionAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Este préstamo ya fue devuelto previamente.", result.Message);
        }

        [Fact]
        public async Task RegistrarDevolucionAsync_ShouldReturnSuccess_WhenValidReturn()
        {
            // Arrange
            var dto = new RegistrarDevolucionDto { IdPrestamo = 1, FechaDevolucion = DateTime.Now };
            var prestamo = new Prestamo(1, "1234567890123", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(2));

            _repoMock.Setup(r => r.GetByIdAsync(dto.IdPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));
            _repoMock.Setup(r => r.UpdateAsync(prestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Success(prestamo));

            // Act
            var result = await _service.RegistrarDevolucionAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Devolución registrada correctamente.", result.Data);
        }

        


       

       
    }
}