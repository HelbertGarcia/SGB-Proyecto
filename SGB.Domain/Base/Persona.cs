using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SGB.Domain.Base
{
    public abstract class Persona : BaseEntityFecha, IEstaActivo
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public int IdRol { get; private set; }
        public bool EstaActivo { get ; set ; }

        private Persona() : base() { }

        public Persona(string nombre, string apellido, string email, string passwordHash, int idRol) : base()
        {
            ValidarYAsignarNombre(nombre);
            ValidarYAsignarApellido(apellido);
            ValidarYAsignarEmail(email);
            AsignarPasswordHash(passwordHash);
            ValidarYAsignarRol(idRol);
        }

        public void ActualizarDatos(string nuevoNombre, string nuevoApellido, string nuevoEmail)
        {
            ValidarYAsignarNombre(nuevoNombre);
            ValidarYAsignarApellido(nuevoApellido);
            ValidarYAsignarEmail(nuevoEmail);
            ActualizarFechaModificacion(); 
        }

        public void CambiarPasswordHash(string nuevoPasswordHash)
        {
            AsignarPasswordHash(nuevoPasswordHash);
            ActualizarFechaModificacion();
        }

        public void CambiarRol(int nuevoIdRol)
        {
            ValidarYAsignarRol(nuevoIdRol);
            ActualizarFechaModificacion(); 
        }

        private void ValidarYAsignarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length > 100)
                throw new ArgumentException("El nombre es inválido.", nameof(nombre));
            Nombre = nombre;
        }

        private void ValidarYAsignarApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido) || apellido.Length > 100)
                throw new ArgumentException("El apellido es inválido.", nameof(apellido));
            Apellido = apellido;
        }

        private void ValidarYAsignarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(email));
            Email = email;
        }

        private void AsignarPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El hash de la contraseña no puede estar vacío.", nameof(passwordHash));
            PasswordHash = passwordHash;
        }

        private void ValidarYAsignarRol(int idRol)
        {
            if (idRol <= 0)
                throw new ArgumentException("El Id del rol es inválido.", nameof(idRol));
            IdRol = idRol;
        }

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
