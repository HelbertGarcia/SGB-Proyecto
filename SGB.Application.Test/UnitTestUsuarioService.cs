using Microsoft.Extensions.Logging;
using Moq;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Application.Services.UsuarioServices;
using SGB.Domain.Base;
namespace SGB.Application.Test
{
    public class UnitUsuarioServiceTest
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly Mock<IUsuarioServices> _repoMock;
        private readonly Mock<ILogger<UsuarioService>> _loggerMock;
        private readonly UsuarioService _service;

        public UnitUsuarioServiceTest()
        {
            _repoMock = new Mock<IUsuarioServices>();
            _loggerMock = new Mock<ILogger<UsuarioService>>();
            _service = new UsuarioService(_repoMock.Object, Mock.Of<ILoggerFactory>(), Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>());
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenInputIsValid()
        {
            // Arrange
            var dto = new SaveUsuarioDto
            {
                Nombre = "Juan",
                Email = "juan@example.com",
                PasswordHash = "1234",
                IDRol = 1
            };

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Usuario creado exitosamente.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenUsuarioExists()
        {
            // Arrange
            var updateDto = new UpdateUsuarioDto
            {
                Nombre = "Pedro",
                Email = "pedro@example.com",
                PasswordHash = "5678",
                IDRol = 2,
                EstaActivo = true
            };

            // Act
            var result = await _service.UpdateAsync(1, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Usuario actualizado exitosamente.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenUsuarioDeleted()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _service.DeleteAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Usuario eliminado correctamente.", result.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnSuccess_WhenUsuariosExist()
        {
            // Act
            var result = await _service.GetAllUsuario();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("DemoUsuario", result.Data.First().Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess_WhenUsuarioExists()
        {
            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("DemoIdUsuario", result.Data.Nombre);
        }

        [Fact]
        public async Task BuscarUsuariosAsync_ShouldReturnSuccess_WhenTerminoEsVacio()
        {
            // Act
            var result = await _service.BuscarUsuariosAsync("");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("DemoUsuario", result.Data.First().Nombre);
        }

        
      
    }
}
