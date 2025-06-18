using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Usuario
{

    public class Usuario : Persona
    {
        public Usuario(string nombre, string apellido, string correo, string contraseña, int idRol)
            : base(nombre, apellido, correo, contraseña, idRol)
        {
        }

        protected Usuario(int id, string nombre, string apellido, string correo, string contraseña, int idRol)
            : base(id, nombre, apellido, correo, contraseña, idRol)
        {
        }
    }

}

