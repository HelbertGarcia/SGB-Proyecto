using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Base
{
    public interface IEstaActivo
    {
        public bool EstaActivo { get; } 

        public void Deshabilitar();

        public void Habilitar();

    }
}
