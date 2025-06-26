using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.NotificacionesDto
{
    public record GetNotificacionDto(
        int Id,
        string Tipo,
        string Mensaje,
        DateTime Fecha,
        bool Leida
    );
}
