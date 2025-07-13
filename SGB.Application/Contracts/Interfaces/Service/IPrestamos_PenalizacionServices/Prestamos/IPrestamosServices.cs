using SGB.Application.Base;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Interfaces.Service.IPrestamos_PenalizacionServices.Prestamos
{
    public interface IPrestamosServices : IBaseService<AddPrestamoDto, UpdatePrestamoDto, DiseblePrestamoDto>
    {




        // RF3.2: Controlar la fecha de vencimiento (actualiza estado de préstamo si está vencido)
        Task<OperationResult> ActualizarEstadoPrestamoPorVencimientoAsync(int idPrestamo);

        // RF3.3: Registrar la devolución de un libro
        Task<OperationResult> RegistrarDevolucionAsync(int idPrestamo);

        // RF3.5: Verificar si un usuario puede hacer préstamos (sin penalizaciones activas ni libros pendientes)
        Task<bool> PuedePrestarAsync(int usuarioId);

        // Consultar préstamos activos de un usuario
        Task<OperationResult> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId);



    }
}
