using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Extensions;
using SGB.Domain.Base;
using System.Text.Json;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Infraestructure.Loggers
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Info(string message, params object[] args)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("[{ClassName}] {Message}",
                    typeof(T).Name,
                    FormatMessage(message, args));
            }
        }

        public void Error(string message, params object[] args)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError("[{ClassName}] {Message}",
                    typeof(T).Name,
                    FormatMessage(message, args));
            }
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(exception, "[{ClassName}] {Message}",
                    typeof(T).Name,
                    FormatMessage(message, args));
            }
        }

        /// <summary>
        /// Formatea el mensaje con los argumentos de forma segura
        /// </summary>
        private string FormatMessage(string message, object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return string.Empty;

            if (args == null || args.Length == 0)
                return message;

            try
            {
                // Convertir argumentos complejos a JSON de forma segura
                var serializedArgs = args.Select(SafeSerialize).ToArray();
                return string.Format(message, serializedArgs);
            }
            catch (FormatException)
            {
                // Si falla el formateo, devolver el mensaje original con los args serializados
                var argsString = string.Join(", ", args.Select(SafeSerialize));
                return $"{message} [Args: {argsString}]";
            }
            catch (Exception ex)
            {
                // En caso de error inesperado, log básico
                _logger.LogWarning(ex, "Error formatting log message: {OriginalMessage}", message);
                return $"{message} [Formatting failed]";
            }
        }

        /// <summary>
        /// Serializa un objeto de forma segura para logging
        /// </summary>
        private string SafeSerialize(object obj)
        {
            if (obj == null)
                return "null";

            try
            {
                // Para tipos primitivos y string, usar ToString()
                if (obj is string || obj.GetType().IsPrimitive || obj is decimal || obj is DateTime)
                    return obj.ToString();

                // Para objetos complejos, usar JSON
                var options = new JsonSerializerOptions
                {
                    WriteIndented = false,
                    MaxDepth = 3 // Evitar referencias circulares profundas
                };

                return JsonSerializer.Serialize(obj, options);
            }
            catch (JsonException)
            {
                // Si falla la serialización JSON, usar ToString()
                return obj.ToString() ?? obj.GetType().Name;
            }
            catch (Exception)
            {
                // Último recurso
                return $"[{obj.GetType().Name}]";
            }


        }
    }
}
