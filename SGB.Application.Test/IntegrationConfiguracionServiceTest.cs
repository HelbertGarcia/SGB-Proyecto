using Moq;
using SGB.Persistence.Context;
using SGB.Persistence.Repositories;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Api.Services.ConfiguracionServices;
using SGB.Api.Extensions.Mappers.ConfiguracionMapper;
using SGB.Api.Validators.BusinessValidators.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static SGB.Api.Extensions.Loggin.LoggerExtensions;

namespace SGB.Api.Test.IntegrationTests
{
    public class IntegrationConfiguracionServiceTest
    {
        private ConfiguracionService CreateService(out SGBContext context)
        {
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new SGBContext(options);

            var configuration = new ConfigurationBuilder().Build();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var loggerRepo = new Mock<IAppLogger<ConfiguracionRepository>>().Object;
            var loggerService = new Mock<IAppLogger<ConfiguracionService>>().Object;

            var repo = new ConfiguracionRepository(context, loggerFactory, configuration, loggerRepo);
            var mapper = new ConfiguracionMapper();
            var validator = new ConfiguracionValidator(repo);

            return new ConfiguracionService(repo, mapper, validator, configuration, loggerService);
        }

        [Fact]
        public async Task FlujoCompletoConfiguracion_DeberiaFuncionarBien()
        {
            var service = CreateService(out var context);
            var addDto = new AddConfiguracionDto
            {
                Nombre = "IntegraciónTest",
                Valor = "Valor Inicial",
                Descripcion = "Prueba de integración"
            };
            var createResult = await service.AddAsync(addDto);

            Assert.True(createResult.IsSuccess);
            var createdId = createResult.Data.IDConfiguracion;
            Assert.Equal("IntegraciónTest", createResult.Data.Nombre);

            var getResult = await service.GetByIdAsync(createdId);
            Assert.True(getResult.IsSuccess);
            Assert.Equal("IntegraciónTest", getResult.Data.Nombre);
            Assert.Equal("Valor Inicial", getResult.Data.Valor);
            Assert.Equal("Prueba de integración", getResult.Data.Descripcion);

            var updateDto = new UpdateConfiguracionDto
            {
                IDConfiguracion = createdId,
                Valor = "Valor Actualizado",
                Descripcion = "Descripción actualizada",
                EstaActivo = true
            };
            var updateResult = await service.UpdateAsync(createdId, updateDto);
            Assert.True(updateResult.IsSuccess);
            Assert.Equal("IntegraciónTest", updateResult.Data.Nombre);
            Assert.Equal("Valor Actualizado", updateResult.Data.Valor);
            Assert.Equal("Descripción actualizada", updateResult.Data.Descripcion);
            var deleteResult = await service.DeleteAsync(createdId);
            Assert.True(deleteResult.IsSuccess);
        }

        [Fact]
        public async Task FlujoCompletoConfiguracion_DeberiaValidarMapperYReglas()
        {
            var options = new DbContextOptionsBuilder<SGBContext>()
                .UseInMemoryDatabase($"ConfiguracionTest_{Guid.NewGuid()}")
                .Options;

            var context = new SGBContext(options);
            var configuration = new ConfigurationBuilder().Build();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());

            var loggerRepo = new Mock<IAppLogger<ConfiguracionRepository>>().Object;
            var loggerService = new Mock<IAppLogger<ConfiguracionService>>().Object;

            var repo = new ConfiguracionRepository(context, loggerFactory, configuration, loggerRepo);
            var mapper = new ConfiguracionMapper();
            var validator = new ConfiguracionValidator(repo);

            var service = new ConfiguracionService(repo, mapper, validator, configuration, loggerService);

            var addDto = new AddConfiguracionDto
            {
                Nombre = "CorreoEnvio",
                Valor = "noreply@sgb.local",
                Descripcion = "Configuración de email saliente"
            };

            var createResult = await service.AddAsync(addDto);
            Assert.True(createResult.IsSuccess);
            var idCreado = createResult.Data.IDConfiguracion;

            var resultDuplicado = await service.AddAsync(addDto);
            Assert.False(resultDuplicado.IsSuccess);
            Assert.Equal("Ya existe una configuración con ese nombre.", resultDuplicado.Message);

            var getResult = await service.GetByIdAsync(idCreado);
            Assert.True(getResult.IsSuccess);
            Assert.Equal("CorreoEnvio", getResult.Data.Nombre);

            var otra = new AddConfiguracionDto
            {
                Nombre = "LímiteMensual",
                Valor = "300",
                Descripcion = "Valor máximo mensual"
            };
            var otraResult = await service.AddAsync(otra);

            var updateFail = await service.UpdateAsync(otraResult.Data.IDConfiguracion, new UpdateConfiguracionDto
            {
                IDConfiguracion = otraResult.Data.IDConfiguracion,
                Nombre = "CorreoEnvio",
                Valor = "400",
                Descripcion = "Actualización con nombre duplicado",
                EstaActivo = true
            });

            Assert.False(updateFail.IsSuccess);
            Assert.Contains("Ya existe otra configuración", updateFail.Message);
            var protegida = new AddConfiguracionDto
            {
                Nombre = "config_sistema_base",
                Valor = "segura",
                Descripcion = "Protegida contra eliminación"
            };

            var protegidaResult = await service.AddAsync(protegida);
            var deleteResult = await service.DeleteAsync(protegidaResult.Data.IDConfiguracion);
            Assert.False(deleteResult.IsSuccess);
            Assert.Equal("Esta configuración es protegida y no puede eliminarse.", deleteResult.Message);
        }
    }
}
