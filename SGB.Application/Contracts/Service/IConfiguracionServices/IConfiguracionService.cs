using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;

namespace SGB.Application.Contracts.Service.IConfiguracionService
{
    public interface IConfiguracionService
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> SaveAsync(AddConfiguracionDto dto);
        Task<OperationResult> UpdateAsync(UpdateConfiguracionDto dto);
        Task<OperationResult> DeleteAsync(DeleteConfiguracionDto dto);
    }

}

