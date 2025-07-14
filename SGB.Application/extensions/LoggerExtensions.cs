using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace SGB.Application.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Registra un mensaje de error usando un mensaje definido en el archivo de configuración.
        /// </summary>
        public static void LogErrorWithConfigurationMessage(this ILogger logger,
                                                            IConfiguration configuration,
                                                            Exception ex,
                                                            string configKey,
                                                            params object[] args)
        {
            string friendlyErrorMessage = configuration[configKey] ??
                                          configuration["ErrorMessages:Global:UnexpectedError"] ??
                                          "Ocurrió un error inesperado.";

            logger.LogError(ex, "{FriendlyErrorMessage} | Parámetros de contexto: {@Args}", friendlyErrorMessage, args);
        }

        /// <summary>
        /// Registra un mensaje informativo usando un mensaje definido en el archivo de configuración.
        /// </summary>
        public static void LogInformationWithConfigurationMessage(this ILogger logger,
                                                                   IConfiguration configuration,
                                                                   string configKey,
                                                                   params object[] args)
        {
            string infoMessage = configuration[configKey] ??
                                 configuration["InfoMessages:Global:Default"] ??
                                 "Operación completada.";

            logger.LogInformation("{InfoMessage} | Contexto: {@Args}", infoMessage, args);
        }
    }
}