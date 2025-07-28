using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Usuario;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Entities.Configuracion;
using SGB.Domain.Entities.Notificaciones;
using SGB.Domain.Entities.Penalizaciones;
using Microsoft.EntityFrameworkCore;

namespace SGB.Persistence.Context
{
    public class SGBContext : DbContext
    {
        public SGBContext(DbContextOptions<SGBContext> options) : base(options){ }

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

                entity.Property(c => c.IDConfiguracion)
                      .HasColumnName("IDConfiguracion");

                entity.Property(c => c.Nombre)
                      .HasColumnType("nvarchar(100)")
                      .IsRequired();

                entity.HasIndex(c => c.Nombre)
                      .IsUnique();

                entity.Property(c => c.Valor)
                      .HasColumnType("nvarchar(max)")
                      .IsRequired();

                entity.Property(c => c.Descripcion)
                      .HasColumnType("nvarchar(255)");

                entity.Property(c => c.FechaCreacion)
                      .HasColumnType("datetime2")
                      .IsRequired();

                entity.Property(c => c.EstaActivo)
                      .HasColumnType("bit")
                      .IsRequired();

                entity.Ignore(c => c.Id);

                entity.Ignore(c => c.FechaActualizacion);
            });
        }
    }
}