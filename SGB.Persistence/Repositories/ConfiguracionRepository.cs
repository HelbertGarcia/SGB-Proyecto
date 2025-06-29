using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;

namespace SGB.Persistence.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<ConfiguracionRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ConfiguracionRepository(
            SGBContext context,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<ConfiguracionRepository>();
            _connectionString = _configuration.GetConnectionString("SGBDatabase")!;
        }

        #region Métodos Entity Framework

        public async Task AddAsync(Configuracion config)
        {
            await Entity.AddAsync(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntityAsync(Configuracion entity)
        {
            Entity.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntityAsync(Configuracion config)
        {
        }

        public async Task<bool> DeshabilitarAsync(int id)
        {
            var config = await Entity.FindAsync(id);
            if (config == null)
                return false;

            config.Deshabilitar();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Configuracion>> GetAllAsync()
        {
            var configuraciones = new List<Configuracion>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT * FROM Configuracion", connection);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var config = new Configuracion(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3)
                )
                {
                    IDConfiguracion = reader.GetInt32(0),
                    FechaCreacion = reader.GetDateTime(4),
                    EstaActivo = reader.GetBoolean(5)
                };

                configuraciones.Add(config);
            }

            return configuraciones;
        }

        public async Task<Configuracion> GetEntityByIdAsync(int id)
        {
            return await Entity.FindAsync(id);
        }

        public async Task<OperationResult> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                var result = await Entity.AsNoTracking()
                                         .Where(c => c.Nombre == nombre)
                                         .ToListAsync();

                return new OperationResult { Data = result };
            }
            catch (Exception ex)
            {
                const string errorMsg = "Error al buscar configuración por nombre.";
                _logger.LogError(ex, errorMsg);
                return new OperationResult { Success = false, Message = errorMsg };
            }
        }

        #endregion

        #region Métodos con Procedimientos Almacenados

        public async Task<OperationResult> ObtenerConfiguracionAsync(int id)
        {
            var result = new OperationResult();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("usp_Configuracion_Get", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@IDConfiguracion", id);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var config = new Configuracion(
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.IsDBNull(3) ? null : reader.GetString(3)
                    )
                    {
                        IDConfiguracion = reader.GetInt32(0),
                        FechaCreacion = reader.GetDateTime(4),
                        EstaActivo = reader.GetBoolean(5)
                    };

                    result.Success = true;
                    result.Data = config;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Configuración no encontrada.";
                }
            }
            catch (SqlException ex)
            {
                const string msg = "Error SQL en ObtenerConfiguracionAsync.";
                _logger.LogError(ex, msg);
                result.Success = false;
                result.Message = msg;
            }

            return result;
        }

        public async Task<OperationResult> ActualizarConfiguracionAsync(int id, string valor, string descripcion = null, bool? estaActivo = null)
        {
            var result = new OperationResult();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("usp_Configuracion_Modificar", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@IDConfiguracion", id);
                command.Parameters.AddWithValue("@Valor", valor);
                command.Parameters.AddWithValue("@Descripcion", (object?)descripcion ?? DBNull.Value);
                command.Parameters.AddWithValue("@EstaActivo", (object?)estaActivo ?? DBNull.Value);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                result.Success = true;
                result.Message = "Configuración actualizada correctamente.";
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL en ActualizarConfiguracionAsync");
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<OperationResult> DeshabilitarConfiguracionAsync(int id)
        {
            var result = new OperationResult();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("usp_Configuracion_Deshabilitar", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@IDConfiguracion", id);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                result.Success = true;
                result.Message = "Configuración deshabilitada.";
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL en DeshabilitarConfiguracionAsync");
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion
    }
}
