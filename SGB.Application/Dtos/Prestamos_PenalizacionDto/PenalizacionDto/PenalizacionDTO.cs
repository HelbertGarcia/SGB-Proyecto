using SGB.Application.Dtos.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public abstract class PenalizacionDto : DtoBase
    {
        public string Motivo { get; set; } = string.Empty;
        public int UsuarioId { get; set; }

    }

 }

    
