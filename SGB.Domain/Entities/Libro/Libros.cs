using SGB.Domain.Base;
using System;

namespace SGB.Domain.Entities.Libro
{
    public class Libro : BaseEntity
    {
        public string Titulo { get; private set; }
        public string Autor { get; private set; }
        public string Editorial { get; private set; }
        public DateOnly FechaPublicacion { get; private set; }
        public int CategoriaId { get; private set; }

        private Libro() : base() { }

        public Libro(string titulo, string autor, string editorial, DateOnly fechaPublicacion, int categoriaId) : base()
        {
            ValidarYAsignarTitulo(titulo);
            ValidarYAsignarAutor(autor);
            ValidarYAsignarFechaPublicacion(fechaPublicacion);
            ValidarYAsignarCategoria(categoriaId);
            Editorial = editorial;
        }

        public void ActualizarDetalles(string nuevoTitulo, string nuevoAutor, string nuevaEditorial)
        {
            ValidarYAsignarTitulo(nuevoTitulo);
            ValidarYAsignarAutor(nuevoAutor);
            Editorial = nuevaEditorial;
        }

        public void CambiarCategoria(int nuevaCategoriaId)
        {
            ValidarYAsignarCategoria(nuevaCategoriaId);
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

        private void ValidarYAsignarFechaPublicacion(DateOnly fechaPublicacion)
        {
            TimeZoneInfo zonaHorariaSantoDomingo = TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time");
            DateTime ahoraEnSantoDomingo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaHorariaSantoDomingo);

            if (fechaPublicacion > DateOnly.FromDateTime(ahoraEnSantoDomingo))
            {
                throw new ArgumentException("La fecha de publicación no puede ser una fecha futura.", nameof(fechaPublicacion));
            }
            FechaPublicacion = fechaPublicacion;
        }

        private void ValidarYAsignarCategoria(int categoriaId)
        {
            if (categoriaId <= 0)
                throw new ArgumentException("El Id de la categoría es inválido.", nameof(categoriaId));
            CategoriaId = categoriaId;
        }
    }
}