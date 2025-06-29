using Microsoft.EntityFrameworkCore;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Configuracion;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Notificaciones;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Entities.Rol;
using SGB.Domain.Entities.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Context
{
    public class SGBContext: DbContext
    {
        public SGBContext(DbContextOptions<SGBContext> options) : base(options)
        {

        }

        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Notificacion> Notificacion { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Penalizacion> Penalizacion { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Configuracion> Configuracion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Siempre llama a la implementación base primero

            // Configuración para la entidad Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("IDCategoria");
            });

            // Configuración para la jerarquía de Persona (Table-Per-Type)
            modelBuilder.Entity<Persona>().ToTable("Usuario");
            modelBuilder.Entity<Persona>().Property(p => p.Id).HasColumnName("IDUsuario");

            modelBuilder.Entity<Administrador>().ToTable("Administrador");
            modelBuilder.Entity<Bibliotecario>().ToTable("Bibliotecario");

            // Aquí puedes añadir más configuraciones para otras entidades si es necesario...
            // Por ejemplo, para la entidad Libro:
            modelBuilder.Entity<Libro>().HasKey(l => l.ISBN); // Define explícitamente que ISBN es la PK.

            modelBuilder.Entity<Prestamo>(entity =>
            {
                // Le decimos que la tabla en la BD se llama 'Prestamos'
                entity.ToTable("Prestamos");

                // 1. Mapeo de la Clave Primaria
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("IDPrestamo"); // Traduce 'Id' (C#) a 'IDPrestamo' (SQL)

                // 2. Mapeo de las Claves Foráneas y otras propiedades
                // Asumiendo que la propiedad en tu entidad se llama 'LibroIsbn' para mayor claridad.
                entity.Property(p => p.EjemplarId).HasColumnName("ISBN").IsRequired().HasMaxLength(13);
                entity.Property(p => p.UsuarioId).HasColumnName("IDUsuario").IsRequired();

                // 3. Mapeo del Enum a String
                // Esto le dice a EF Core que guarde el nombre del enum (ej. "Activo") como un string en la BD.
                entity.Property(p => p.Estado)
                      .HasConversion<string>()
                      .HasMaxLength(50); // El tamaño de tu columna nvarchar

                // 4. Configuración de las Relaciones (Buena Práctica)
                entity.HasOne<Libro>() // Un Préstamo tiene un Libro
                      .WithMany() // Un Libro puede estar en muchos Préstamos
                      .HasForeignKey(p => p.EjemplarId); // La clave foránea es LibroIsbn (que mapea a la columna ISBN)

                entity.HasOne<Usuario>() // Un Préstamo tiene un Usuario
                      .WithMany() // Un Usuario puede tener muchos Préstamos
                      .HasForeignKey(p => p.UsuarioId);
            });
        }
    }


}
