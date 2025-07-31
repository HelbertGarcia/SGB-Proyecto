using Moq;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Api.Contracts.Mappers;
using SGB.Api.Services.ConfiguracionServices;
using SGB.Api.Contracts.Repository.Interfaces;
using SGB.Api.Validators.BusinessValidators.Configuracion;
using Microsoft.Extensions.Configuration;
using static SGB.Api.Extensions.Loggin.LoggerExtensions;

namespace SGB.Api.Test
{
    public class UnitConfiguracionServiceTest
    {
        private readonly Mock<IConfiguracionRepository> _repoMock;
        private readonly Mock<IConfiguracionMapper> _mapperMock;
        private readonly Mock<IConfiguracionValidator> _validatorMock;
        private readonly Mock<IAppLogger<ConfiguracionService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly ConfiguracionService _service;

        public UnitConfiguracionServiceTest()
        {
            _repoMock = new Mock<IConfiguracionRepository>();
            _mapperMock = new Mock<IConfiguracionMapper>();
            _validatorMock = new Mock<IConfiguracionValidator>();
            _loggerMock = new Mock<IAppLogger<ConfiguracionService>>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c[It.IsAny<string>()]).Returns("Mensaje de configuración simulado");

            _service = new ConfiguracionService(
                _repoMock.Object,
                _mapperMock.Object,
                _validatorMock.Object,
                _configurationMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenNombreOrValorIsEmpty()
        {
            var dto = new AddConfiguracionDto { Nombre = "", Valor = "" };
            var result = await _service.AddAsync(dto);
            Assert.False(result.IsSuccess);
            Assert.Equal("El nombre no puede estar vacío.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenInputIsValid()
        {
            var dto = new AddConfiguracionDto { Nombre = "Nombre", Valor = "Valor", Descripcion = "Desc" };

            _validatorMock.Setup(v => v.ValidarAsync(dto))
                .ReturnsAsync(OperationResult<bool>.Success(true));

            var entity = new Configuracion(dto.Nombre, dto.Valor, dto.Descripcion);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Configuracion>()))
                .ReturnsAsync(OperationResult<Configuracion>.Success(entity));

            _mapperMock.Setup(m => m.MapToDto(entity))
                .Returns(new ConfiguracionDto { Nombre = "Nombre", Valor = "Valor", Descripcion = "Desc" });

            var result = await _service.AddAsync(dto);
            Assert.True(result.IsSuccess);
            Assert.Equal("Configuración registrada exitosamente.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenConfiguracionNotFound()
        {
            var dto = new UpdateConfiguracionDto { IDConfiguracion = 999, Valor = "Nuevo", Descripcion = "Actualizado", EstaActivo = true };
            _repoMock.Setup(r => r.GetByIdAsync(dto.IDConfiguracion))
                .ReturnsAsync(OperationResult<Configuracion>.Failure("No encontrada"));
            var result = await _service.UpdateAsync(dto.IDConfiguracion, dto);
            Assert.False(result.IsSuccess);
            Assert.Equal("Mensaje de configuración simulado", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenIdIsInvalid()
        {
            int invalidId = -1;
            _repoMock.Setup(r => r.GetByIdAsync(invalidId))
                .ReturnsAsync(OperationResult<Configuracion>.Failure("No encontrada"));
            var result = await _service.DeleteAsync(invalidId);
            Assert.False(result.IsSuccess);
            Assert.Equal("Mensaje de configuración simulado", result.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnSuccess_WhenDataExists()
        {
            var entities = new List<Configuracion>
            {
                new Configuracion("Demo", "Valor", "Desc") { IDConfiguracion = 1 }
            };
            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(OperationResult<IEnumerable<Configuracion>>.Success(entities));
            _mapperMock.Setup(m => m.MapToDto(It.IsAny<Configuracion>()))
                .Returns((Configuracion c) => new ConfiguracionDto { Nombre = c.Nombre, Valor = c.Valor });
            var result = await _service.GetAllAsync();
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("Demo", result.Data.First().Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            var entity = new Configuracion("DemoId", "Valor", "Desc") { IDConfiguracion = 1 };
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(OperationResult<Configuracion>.Success(entity));
            _mapperMock.Setup(m => m.MapToDto(entity))
                .Returns(new ConfiguracionDto { Nombre = "DemoId", Valor = "Valor", Descripcion = "Desc" });

            var result = await _service.GetByIdAsync(1);
            Assert.True(result.IsSuccess);
            Assert.Equal("DemoId", result.Data.Nombre);
        }


        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnSuccess_WhenConfiguracionExists()
        {
            var nombre = "ConfigDemo";
            var entidad = new Configuracion(nombre, "ValorDemo", "DescDemo");

            _repoMock.Setup(r => r.ObtenerPorNombreAsync(nombre))
                .ReturnsAsync(OperationResult<Configuracion>.Success(entidad));

            _mapperMock.Setup(m => m.MapToDto(entidad))
                .Returns(new ConfiguracionDto { Nombre = nombre, Valor = "ValorDemo", Descripcion = "DescDemo" });

            var result = await _service.ObtenerPorNombreAsync(nombre);
            Assert.True(result.IsSuccess);
            Assert.Equal(nombre, result.Data.Nombre);
        }

        [Fact]
        public async Task ObtenerPorNombreAsync_ShouldReturnFailure_WhenConfiguracionNotFound()
        {
            var nombre = "NoExiste";

            _repoMock.Setup(r => r.ObtenerPorNombreAsync(nombre))
                .ReturnsAsync(OperationResult<Configuracion>.Failure("No encontrada"));

            var result = await _service.ObtenerPorNombreAsync(nombre);
            Assert.False(result.IsSuccess);
            Assert.Equal("Mensaje de configuración simulado", result.Message);
        }
    }
}
