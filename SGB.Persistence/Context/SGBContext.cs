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

namespace SGB.Persistence.Context
{
    public class SGBContext : DbContext
    {
        public SGBContext(DbContextOptions<SGBContext> options) : base(options)
        {
        }

        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Bibliotecario> Bibliotecarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.ToTable("Configuracion");
                entity.HasKey(c => c.IDConfiguracion);
                entity.Property(c => c.IDConfiguracion).HasColumnName("IDConfiguracion");

                entity.Property(c => c.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Valor)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(c => c.Descripcion)
                      .HasMaxLength(1000);

                entity.Property(c => c.FechaCreacion)
                      .IsRequired();

                entity.Property(c => c.EstaActivo)
                      .IsRequired();
            });
        }

    }
}