using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Usuario
{
    public class Bibliotecario: Persona
    {
        public Bibliotecario(string nombre, string apellido, string email, string passwordHash, int idRol)
        : base(nombre, apellido, email, passwordHash, idRol)
        {

        }
    }
}
