using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Logger
{
    internal interface ILoggingServices
    {
        Task<OperationResult> LogError(string args, object instance, [CallerMemberName] string method = "");
        Task<OperationResult> LogWarning(string args, object instance, [CallerMemberName] string method = "");
    }
}
