using SGB.Application.Base;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contratos.interfaces.LibrosServices
{
    public interface ILibroService: IBaseService<AddLibroDto, UpdateLibroDto, LibroDto>
    {

    }
}
