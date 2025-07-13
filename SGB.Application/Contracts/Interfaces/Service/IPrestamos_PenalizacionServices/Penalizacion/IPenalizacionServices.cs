using SGB.Application.Base;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SGB.Application.Contracts.Interfaces.Service.IPrestamos_PenalizacionServices.Penalizacion
{
    public interface IPenalizacionServices : IBaseService<AddPenalizacionDto, UpdatePenalizacionDto, DisablePenalizacionDto>
    {

        // RF3.4: Calcular penalizaciones por retraso (por ejemplo al registrar devolución)
        Task<OperationResult> CalcularPenalizacionPorRetrasoAsync(int idPrestamo);


        // Obtener penalizaciones activas de un usuario
        Task<OperationResult> ObtenerPenalizacionesActivasPorUsuarioAsync(int usuarioId);
    }



}

