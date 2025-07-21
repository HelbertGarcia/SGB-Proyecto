using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Extensions.Loggin
{
    public static class LoggerExtensions
    {
        public interface IAppLogger<T>
        {
            void Info(string message, params object[] args);
            void Error(string message, params object[] args);
            void Error(Exception exception, string message, params object[] args);
        }
    }
}
