using Microsoft.Extensions.Logging;
using Moq;
using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;

using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SGB.Application.Test.ValidatorTests
{
    public class PenalizacionBusinessValidatorTest
    {
        private readonly Mock<IPenalizacionRepository> _penalizacionRepoMock;
        private readonly Mock<IPrestamoRepository> _prestamoRepoMock;
        private readonly PenalizacionBusinessValidator _validator;

        public PenalizacionBusinessValidatorTest()
        {
            _penalizacionRepoMock = new Mock<IPenalizacionRepository>();
            _prestamoRepoMock = new Mock<IPrestamoRepository>();
            _validator = new PenalizacionBusinessValidator(_penalizacionRepoMock.Object, _prestamoRepoMock.Object);
        }

        [Fact]
        public async Task ValidateForAddAsync_ShouldReturnFailure_WhenDtoIsNull()
        {
            // Arrange
            AddPenalizacionDto dto = null;

            // Act
            var result = await _validator.ValidateForAddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Datos inválidos para penalización.", result.Message);
        }

        [Fact]
        public async Task ValidateForAddAsync_ShouldReturnFailure_WhenFechaInicioIsGreaterThanFechaFin()
        {
            // Arrange
            var dto = new AddPenalizacionDto
            {
                UsuarioId = 1,
                Motivo = "Retraso en devolución",
                FechaInicio = DateTime.Now.AddDays(5),
                FechaFin = DateTime.Now.AddDays(1),
                Monto = 100.00m,
                IDPrestamo = 1
            };

            // Act
            var result = await _validator.ValidateForAddAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La fecha de inicio no puede ser mayor que la fecha fin.", result.Message);
        }

       
       
        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnFailure_WhenDtoIsNull()
        {
            // Arrange
            UpdatePenalizacionDto dto = null;

            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Datos inválidos para actualizar penalización.", result.Message);
        }

        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnFailure_WhenPenalizacionNotFound()
        {
            // Arrange
            var dto = new UpdatePenalizacionDto
            {
                IDPenalizacion = 999,
                Motivo = "Motivo actualizado"
            };

            _penalizacionRepoMock.Setup(r => r.GetByIdAsync(dto.IDPenalizacion))
                .ReturnsAsync(OperationResult<Penalizacion>.Failure("No encontrada"));

            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Penalización no encontrada.", result.Message);
        }

        [Fact]
        public async Task ValidateForUpdateAsync_ShouldReturnFailure_WhenFechaInicioIsGreaterThanFechaFin()
        {
            // Arrange
            var dto = new UpdatePenalizacionDto
            {
                IDPenalizacion = 1,
                FechaInicio = DateTime.Now.AddDays(5),
                FechaFin = DateTime.Now.AddDays(1)
            };

            var penalizacion = new Penalizacion(1, "Motivo", DateTime.Now, DateTime.Now.AddDays(7), 1, 100.00m);
            _penalizacionRepoMock.Setup(r => r.GetByIdAsync(dto.IDPenalizacion))
                .ReturnsAsync(OperationResult<Penalizacion>.Success(penalizacion));

            // Act
            var result = await _validator.ValidateForUpdateAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La fecha de inicio no puede ser mayor que la fecha fin.", result.Message);
        }

        [Fact]
        public async Task ValidateForDisableAsync_ShouldReturnFailure_WhenPenalizacionAlreadyDisabled()
        {
            // Arrange
            var dto = new DisablePenalizacionDto { IDPenalizacion = 1 };
            var penalizacion = new Penalizacion(1, "Motivo", DateTime.Now, DateTime.Now.AddDays(7), 1, 100.00m);
            penalizacion.Deshabilitar();

            _penalizacionRepoMock.Setup(r => r.GetByIdAsync(dto.IDPenalizacion))
                .ReturnsAsync(OperationResult<Penalizacion>.Success(penalizacion));

            // Act
            var result = await _validator.ValidateForDisableAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La penalización ya está desactivada.", result.Message);
        }

        [Fact]
        public async Task ValidateForCalcularPenalizacionAsync_ShouldReturnFailure_WhenPrestamoNotFound()
        {
            // Arrange
            int idPrestamo = 999;
            _prestamoRepoMock.Setup(r => r.GetByIdAsync(idPrestamo))
                .ReturnsAsync(OperationResult<Prestamo>.Failure("No encontrado"));

            // Act
            var result = await _validator.ValidateForCalcularPenalizacionAsync(idPrestamo);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Préstamo no encontrado.", result.Message);
        }
    }
}