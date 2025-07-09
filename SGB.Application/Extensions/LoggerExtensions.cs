using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Loggers
{
    public  static class LoggerExtensions
    {
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
    }
}
