

using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
       
        // Obtiene la fecha de vencimiento FechaFin de un prestamo por su ID.
        
        Task<OperationResult<DateTime>> GetFechaVencimientoByPrestamoIdAsync(int prestamoId);

     
       
        //Lista de préstamos activos y atrasados de un usuario.
       
        Task<OperationResult<List<Prestamo>>> GetPrestamosActivosPorUsuarioAsync(int usuarioId);

 
        
    }
}
