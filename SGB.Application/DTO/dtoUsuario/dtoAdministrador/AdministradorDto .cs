using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Persistence.Interfaces;

namespace SGB.Application.DTO.dtoUsuario.dtoAdministrador
{
    public class AdministradorDto : IFechaActualizacion, IEstaActivo
    {
        public int IDAdmin { get; set; }
        public int IDUsuario { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool EstaActivo { get; set; }
        public DateTime FechaActualizacion { get; set; }



    }
}
