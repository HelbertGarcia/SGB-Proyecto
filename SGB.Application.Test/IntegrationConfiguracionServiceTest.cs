using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Services.ConfiguracionServices;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using Xunit;

namespace SGB.Application.Test.IntegrationTests
{
    public class IntegrationConfiguracionServiceTest
    {
        private ConfiguracionService CreateService(out SGBContext context)
        {
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(databaseName: "Integration_DB")
                .Options;

            context = new SGBContext(options);

            var configMock = new ConfigurationBuilder().Build();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var repo = new ConfiguracionRepository(context, loggerFactory, configMock);

            var loggerService = loggerFactory.CreateLogger<ConfiguracionService>();
            return new ConfiguracionService(repo, loggerService);
        }

        [Fact]
        public async Task FullConfiguracionFlow_ShouldWorkCorrectly()
        {
            // Arrange
            var service = CreateService(out var context);

            var addDto = new AddConfiguracionDto
            {
                Nombre = "IntegraciónTest",
                Valor = "Valor Inicial",
                Descripcion = "Prueba de integración"
            };

            // Act - Create
            var createResult = await service.AddAsync(addDto);
            Assert.True(createResult.IsSuccess);

            var createdId = createResult.Data.IDConfiguracion;

            // Act - Read
            var getResult = await service.GetByIdAsync(createdId);
            Assert.True(getResult.IsSuccess);
            Assert.Equal("IntegraciónTest", getResult.Data.Nombre);

            // Act - Update
            var updateDto = new UpdateConfiguracionDto
            {
                IDConfiguracion = createdId,
                Valor = "Valor Actualizado",
                Descripcion = "Descripción actualizada",
                EstaActivo = true
            };

            var updateResult = await service.UpdateAsync(createdId, updateDto);
            Assert.True(updateResult.IsSuccess);
            Assert.Equal("Valor Actualizado", updateResult.Data.Valor);

            // Act - Delete
            var deleteResult = await service.DeleteAsync(createdId);
            Assert.True(deleteResult.IsSuccess);

            // Assert - Confirm soft delete
            var finalCheck = await context.Configuraciones.FindAsync(createdId);
            Assert.False(finalCheck.EstaActivo);
        }
    }
}
