using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities
{

        public class Usuario : BaseEntity
        {
            public string Nombre { get; private set; }
            public string Apellido { get; private set; }
            public string Correo { get; private set; }
            public string Contraseña { get; private set; }
            public int IdRol { get; private set; }

            private Usuario() : base() { }

            public Usuario(string nombre, string apellido, string correo, string contraseña, int idRol) : base()
            {
                ValidarYAsignarNombre(nombre);
                ValidarYAsignarApellido(apellido);
                ValidarYAsignarCorreo(correo);
                ValidarYAsignarContraseña(contraseña);
                ValidarYAsignarRol(idRol);
            }

            public void ActualizarDatos(string nuevoNombre, string nuevoApellido, string nuevoCorreo)
            {
                ValidarYAsignarNombre(nuevoNombre);
                ValidarYAsignarApellido(nuevoApellido);
                ValidarYAsignarCorreo(nuevoCorreo);
            }

            public void CambiarContraseña(string nuevaContraseña)
            {
                ValidarYAsignarContraseña(nuevaContraseña);
            }

            public void CambiarRol(int nuevoIdRol)
            {
                ValidarYAsignarRol(nuevoIdRol);
            }

            private void ValidarYAsignarNombre(string nombre)
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
                if (nombre.Length > 50)
                    throw new ArgumentException("El nombre no puede tener más de 50 caracteres.", nameof(nombre));
                Nombre = nombre;
            }

            private void ValidarYAsignarApellido(string apellido)
            {
                if (string.IsNullOrWhiteSpace(apellido))
                    throw new ArgumentException("El apellido no puede estar vacío.", nameof(apellido));
                if (apellido.Length > 50)
                    throw new ArgumentException("El apellido no puede tener más de 50 caracteres.", nameof(apellido));
                Apellido = apellido;
            }

            private void ValidarYAsignarCorreo(string correo)
            {
                if (string.IsNullOrWhiteSpace(correo))
                    throw new ArgumentException("El correo no puede estar vacío.", nameof(correo));
                if (!correo.Contains("@") || !correo.Contains("."))
                    throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(correo));
                Correo = correo;
            }

            private void ValidarYAsignarContraseña(string contraseña)
            {
                if (string.IsNullOrWhiteSpace(contraseña))
                    throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contraseña));
                if (contraseña.Length < 6)
                    throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.", nameof(contraseña));
                Contraseña = contraseña;
            }

            private void ValidarYAsignarRol(int idRol)
            {
                if (idRol <= 0)
                    throw new ArgumentException("El Id del rol es inválido.", nameof(idRol));
                IdRol = idRol;
            }
        }
    }

