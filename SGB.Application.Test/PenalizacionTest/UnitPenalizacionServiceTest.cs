using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FluentValidation;
using Moq;
using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;

namespace SGB.Application.Test.ServiceTests
{
    public class UnitPenalizacionServiceTest
    {
        private readonly Mock<IPenalizacionRepository> _penalizacionRepoMock;
        private readonly Mock<IPrestamoRepository> _prestamoRepoMock;
        private readonly Mock<ILogger<PenalizacionService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IValidator<AddPenalizacionDto>> _addValidatorMock;
        private readonly Mock<IValidator<UpdatePenalizacionDto>> _updateValidatorMock;
        private readonly Mock<IValidator<DisablePenalizacionDto>> _disableValidatorMock;
        private readonly Mock<IPenalizacionBusinessValidator> _businessValidatorMock;
        private readonly Mock<IPenalizacionMapper> _mapperMock;
        private readonly PenalizacionService _service;

        public UnitPenalizacionServiceTest()
        {
            _penalizacionRepoMock = new Mock<IPenalizacionRepository>();
            _prestamoRepoMock = new Mock<IPrestamoRepository>();
            _loggerMock = new Mock<ILogger<PenalizacionService>>();
            _configurationMock = new Mock<IConfiguration>();
            _addValidatorMock = new Mock<IValidator<AddPenalizacionDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdatePenalizacionDto>>();
            _disableValidatorMock = new Mock<IValidator<DisablePenalizacionDto>>();
            _businessValidatorMock = new Mock<IPenalizacionBusinessValidator>();
            _mapperMock = new Mock<IPenalizacionMapper>();

            _service = new PenalizacionService(
                _penalizacionRepoMock.Object,
                _prestamoRepoMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _addValidatorMock.Object,
                _updateValidatorMock.Object,
                _disableValidatorMock.Object,
                _businessValidatorMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var dto = new AddPenalizacionDto
            {
                UsuarioId = 1,
                Motivo = "Retraso",
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(7),
                Monto = 100.00m,
                IDPrestamo = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("UsuarioId", "Error de validación"));

            _addValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error de validación", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenValidInput()
        {
            // Arrange
            var dto = new AddPenalizacionDto
            {
                UsuarioId = 1,
                Motivo = "Retraso en devolución",
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(7),
                Monto = 100.00m,
                IDPrestamo = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _addValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            _businessValidatorMock.Setup(v => v.ValidateForAddAsync(dto))
                .ReturnsAsync(OperationResult<string>.Success("Validación exitosa"));

            var penalizacion = new Penalizacion(dto.UsuarioId, dto.Motivo, dto.FechaInicio, dto.FechaFin, dto.IDPrestamo, dto.Monto);
            _mapperMock.Setup(m => m.MapFromDto(dto))
                .Returns(penalizacion);

            _penalizacionRepoMock.Setup(r => r.AddAsync(It.IsAny<Penalizacion>()))
                .ReturnsAsync(OperationResult<Penalizacion>.Success(penalizacion));

            var responseDto = new PenalizacionResponseDto();
            _mapperMock.Setup(m => m.MapToDto(penalizacion))
                .Returns(responseDto);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Penalización registrada correctamente.", result.Message);
        }

       
        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenValidUpdate()
        {
            // Arrange
            var dto = new UpdatePenalizacionDto
            {
                IDPenalizacion = 1,
                Motivo = "Motivo actualizado",
                FechaFin = DateTime.Now.AddDays(10)
            };

            var penalizacion = new Penalizacion(1, "Motivo original", DateTime.Now, DateTime.Now.AddDays(7), 1, 100.00m);

            var validationResult = new FluentValidation.Results.ValidationResult();
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            _businessValidatorMock.Setup(v => v.ValidateForUpdateAsync(dto))
                .ReturnsAsync(OperationResult<string>.Success("Validación exitosa"));

            _penalizacionRepoMock.Setup(r => r.GetByIdAsync(dto.IDPenalizacion))
                .ReturnsAsync(OperationResult<Penalizacion>.Success(penalizacion));

            _mapperMock.Setup(m => m.ApplyUpdateDto(penalizacion, dto));

            _penalizacionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Penalizacion>()))
                .ReturnsAsync(OperationResult<Penalizacion>.Success(penalizacion));

            var responseDto = new PenalizacionResponseDto();
            _mapperMock.Setup(m => m.MapToDto(penalizacion))
                .Returns(responseDto);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Penalización actualizada correctamente.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var dto = new DisablePenalizacionDto { IDPenalizacion = 1 };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("IDPenalizacion", "Error de validación"));

            _disableValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _service.DeleteAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error de validación", result.Message);
        }



       
       
   

        [Fact]
        public async Task CalcularPenalizacionPorRetrasoAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            int idPrestamo = 1;
            _businessValidatorMock.Setup(v => v.ValidateForCalcularPenalizacionAsync(idPrestamo))
                .ReturnsAsync(OperationResult<string>.Failure("Error de validación"));

            // Act
            var result = await _service.CalcularPenalizacionPorRetrasoAsync(idPrestamo);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Error de validación", result.Message);
        }

       
    }
}