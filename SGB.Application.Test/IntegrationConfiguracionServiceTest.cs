/*using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Test.IntegrationTests
{
    public class IntegrationConfiguracionServiceTest
    {
        private ConfiguracionService CreateService(out SGBContext context)
        {
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new SGBContext(options);

            var configMock = new ConfigurationBuilder().Build();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var mockLoggerRepo = new Mock<IAppLogger<ConfiguracionRepository>>().Object;
            var mockLoggerService = new Mock<IAppLogger<ConfiguracionService>>().Object;

            var repo = new ConfiguracionRepository(context, loggerFactory, configMock, mockLoggerRepo);
            return new ConfiguracionService(repo, mockLoggerService);
        }

            [Fact]
        public async Task FullConfiguracionFlow_ShouldWorkCorrectly()
        {
            // Arrange: instanciar servicio y DTO inicial
            var service = CreateService(out var context);
            var addDto = new AddConfiguracionDto
            {
                Nombre = "IntegraciónTest",
                Valor = "Valor Inicial",
                Descripcion = "Prueba de integración"
            };

            // Act - Crear configuración
            var createResult = await service.AddAsync(addDto);

            // Assert - Validar creación
            Assert.True(createResult.IsSuccess);
            var createdId = createResult.Data.IDConfiguracion;
            Assert.Equal("IntegraciónTest", createResult.Data.Nombre); 

            // Act - Obtener por ID
            var getResult = await service.GetByIdAsync(createdId);

            // Assert - Validar lectura
            Assert.True(getResult.IsSuccess);
            Assert.Equal("DemoId", getResult.Data.Nombre);
            Assert.Equal("ValorId", getResult.Data.Valor);
            Assert.Equal("Dato simulado por ID", getResult.Data.Descripcion);

            // Act - Actualizar configuración
            var updateDto = new UpdateConfiguracionDto
            {
                IDConfiguracion = createdId,
                Valor = "Valor Actualizado",
                Descripcion = "Descripción actualizada",
                EstaActivo = true
            };
            var updateResult = await service.UpdateAsync(createdId, updateDto);

            // Assert - Validar actualización
            Assert.True(updateResult.IsSuccess);
            Assert.Equal("Actualizado", updateResult.Data.Nombre);
            Assert.Equal("Valor Actualizado", updateResult.Data.Valor);
            Assert.Equal("Descripción actualizada", updateResult.Data.Descripcion);

            // Act - Eliminar configuración
            var deleteResult = await service.DeleteAsync(createdId);

            // Assert - Validar eliminación
            Assert.True(deleteResult.IsSuccess);
        }
    }
}
*/