using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Application.DTO.dtoUsuario.dtoAdministrador
{
    public class AdministradorDto : IEstaActivo
    {
        public int IDAdmin { get; set; }
        public int IDUsuario { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool EstaActivo { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public void Deshabilitar()
        {
            EstaActivo = false;
        }

        public void Habilitar()
        {
            EstaActivo = true;
        }

    }
}
