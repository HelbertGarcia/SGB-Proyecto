/*

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Contracts.Logger;
using SGB.Domain.Base;


namespace SGB.Infraestructure.Loggers
    
{
    
    public class LoggingServices : ILoggingServices
    {
        private readonly ILogger<LoggingServices> _logger;
        private readonly IConfiguration _configuration;

        public LoggingServices(ILogger<LoggingServices> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> LogError(string args, object instance, [CallerMemberName] string method = "")
        {
            OperationResult result = new();
            string message = resolveMessage(instance, method, "Error");
            _logger.LogError(message, args);
            result.Message = message;
            result.IsSuccess = false;
            return result;
        }


        public async Task<OperationResult> LogWarning(string args, object instance, [CallerMemberName] string method = "")
        {
            OperationResult result = new();
            string message = resolveMessage(instance, method, "Warning");
            _logger.LogError(message, args);
            result.Message = message;
            result.IsSuccess = false;
            return result;
        }

        private string resolveMessage(object instance, string methodName, string type)
        {
            string className = instance.GetType().Name;
            string completeNameForConfig = $"{type}{className}:{methodName}";
            string? message = _configuration[completeNameForConfig];
            return message ?? $"Undefined message for {completeNameForConfig}";
        }
    }
}
*/