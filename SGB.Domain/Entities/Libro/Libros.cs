using SGB.Domain.Base;
using System;

namespace SGB.Domain.Entities.Libro
{
    public class Libro: IEstaActivo
    {
        public string ISBN { get; private set; }
        public string Titulo { get; private set; }
        public string Autor { get; private set; }
        public string Editorial { get; private set; }
        public DateTime? FechaPublicacion { get; private set; } 

        public int IDCategoria { get; private set; }
        public DateTime FechaRegistro { get; private set; } 
        public DateTime FechaActualizacion { get; private set; }

        public bool EstaActivo { get ; set ; }

       

        public Libro(string isbn, string titulo, string autor, string editorial, DateTime? fechaPublicacion, int idCategoria)
        {
            ValidarYAsignarISBN(isbn);
            ValidarYAsignarTitulo(titulo);
            ValidarYAsignarAutor(autor);
            ValidarYAsignarCategoria(idCategoria);
            Habilitar();

            if (fechaPublicacion.HasValue)
            {
                ValidarYAsignarFechaPublicacion(fechaPublicacion.Value);
            }

            Editorial = editorial;
            FechaRegistro = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void ActualizarDetalles(string nuevoTitulo, string nuevoAutor, string nuevaEditorial, DateTime? nuevaFecha, int nuevaCategoriaId)
        {
            ValidarYAsignarTitulo(nuevoTitulo);
            ValidarYAsignarAutor(nuevoAutor);
            ValidarYAsignarCategoria(nuevaCategoriaId);

            if (nuevaFecha.HasValue)
            {
                ValidarYAsignarFechaPublicacion(nuevaFecha.Value);
            }

            Editorial = nuevaEditorial;

            ActualizarFechaModificacion();
        }

        private void ActualizarFechaModificacion()
        {
            FechaActualizacion = DateTime.UtcNow;
        }


        private void ValidarYAsignarISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn) || (isbn.Length != 10 && isbn.Length != 13))
                throw new ArgumentException("El ISBN debe tener 10 o 13 caracteres.", nameof(isbn));
            ISBN = isbn;
        }

        private void ValidarYAsignarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título del libro no puede estar vacío.", nameof(titulo));
            Titulo = titulo;
        }

        private void ValidarYAsignarAutor(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
                throw new ArgumentException("El autor del libro no puede estar vacío.", nameof(autor));
            Autor = autor;
        }

        private void ValidarYAsignarFechaPublicacion(DateTime fechaPublicacion)
        {
            if (fechaPublicacion.Date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("La fecha de publicación no puede ser una fecha futura.", nameof(fechaPublicacion));
            }
            FechaPublicacion = fechaPublicacion;
        }

        private void ValidarYAsignarCategoria(int idCategoria)
        {
            if (idCategoria <= 0)
                throw new ArgumentException("El Id de la categoría es inválido.", nameof(idCategoria));
            IDCategoria = idCategoria;
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