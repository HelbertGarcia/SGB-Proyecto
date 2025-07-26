using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using SGB.Domain.Entities.Usuario;

using SGB.Application.Contracts.Repository.Interfaces;


namespace SGB.Persistence.Test
{
    public class UnitPersonaRepositoryTests
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly DbContextOptions<SGBContext> _dbOptions;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<PersonaRepository>> _loggerMock;

        public UnitPersonaRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<PersonaRepository>>();
        }

        private PersonaRepository CreateRepository(SGBContext context)
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(l => l.CreateLogger(It.IsAny<string>()))
                             .Returns(_loggerMock.Object);

            return new PersonaRepository(context, loggerFactoryMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task ObtenerPorEmailAsync_ShouldReturnFailure_WhenEmailIsEmpty()
        {
            using var context = new SGBContext(_dbOptions);
            var repo = CreateRepository(context);

            var result = await repo.ObtenerPorEmailAsync("");

            Assert.False(result.IsSuccess);
            Assert.Equal("El correo electrónico no puede estar vacío.", result.Message);
        }

        [Fact]
        public async Task ObtenerPorEmailAsync_ShouldReturnSuccess_WhenEmailExists()
        {
            using var context = new SGBContext(_dbOptions);
            var usuario = new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1); // Proporciona todos los argumentos
            usuario.EstaActivo = true; // Establece otras propiedades según sea necesario
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);
            var result = await repo.ObtenerPorEmailAsync("test@example.com");

            Assert.True(result.IsSuccess);
            Assert.Equal("test@example.com", result.Data.Email);
        }

        [Fact]
        public async Task ExisteEmailAsync_ShouldReturnTrue_WhenEmailExists()
        {
            using var context = new SGBContext(_dbOptions);
            var usuario = new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1); // Proporciona todos los argumentos
            usuario.EstaActivo = true; // Establece otras propiedades según sea necesario
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);
            var result = await repo.ExisteEmailAsync("exists@example.com");

            Assert.True(result.Data);
        }

        [Fact]
        public async Task BuscarPorRolAsync_ShouldReturnUsers_WhenRolExists()
        {
            using var context = new SGBContext(_dbOptions);
            var usuario = new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1); // Proporciona todos los argumentos
            usuario.EstaActivo = true; // Establece otras propiedades según sea necesario
            context.Personas.Add(usuario);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);
            var result = await repo.BuscarPorRolAsync(2);

            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal(2, result.Data.First().IdRol);
        }

        [Fact]
        public async Task ActivarCuentaAsync_ShouldActivate_WhenUserExists()
        {
            using var context = new SGBContext(_dbOptions);
            var usuario = new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1); // Proporciona todos los argumentos
            usuario.EstaActivo = true; // Establece otras propiedades según sea necesario
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);
            var result = await repo.ActivarCuentaAsync(usuario.Id);

            Assert.True(result.IsSuccess);

            var updated = await context.Personas.FindAsync(usuario.Id);
            Assert.True(updated.EstaActivo);
        }

        [Fact]
        public async Task DesactivarCuentaAsync_ShouldDeactivate_WhenUserExists()
        {
            using var context = new SGBContext(_dbOptions);
            var usuario = new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1); // Proporciona todos los argumentos
            usuario.EstaActivo = true; // Establece otras propiedades según sea necesario
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);
            var result = await repo.DesactivarCuentaAsync(usuario.Id);

            Assert.True(result.IsSuccess);

            var updated = await context.Usuarios.FindAsync(usuario.Id);
            Assert.False(updated.EstaActivo);
        }

        [Fact]
        public async Task ObtenerTodosActivosAsync_ShouldReturnOnlyActiveUsers()
        {
            using var context = new SGBContext(_dbOptions);
            context.Usuarios.AddRange(
                new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1),
                new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1),
                new Usuario("NombrePrueba", "ApellidoPrueba", "test@example.com", "PasswordHashPrueba", 1)
            );
            await context.SaveChangesAsync();

            var repo = CreateRepository(context);
            var result = await repo.ObtenerTodosActivosAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }
    }
}
