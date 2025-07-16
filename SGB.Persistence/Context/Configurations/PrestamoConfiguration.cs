using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Entities.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Context.Configurations
{
    public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
    {
        public void Configure(EntityTypeBuilder<Prestamo> entity)
        {
            // Tabla
            entity.ToTable("Prestamos");

            // Clave primaria
            entity.HasKey(p => p.Id);

            // Propiedades simples
            entity.Property(p => p.Id)
                  .HasColumnName("IDPrestamo");

            entity.Property(p => p.ISBN)
                  .HasColumnType("nvarchar(13)")
                  .HasMaxLength(13)
                  .IsRequired();

            entity.Property(p => p.UsuarioId)
                  .HasColumnName("IDUsuario")
                  .IsRequired();

            entity.Property(p => p.FechaInicio)
                  .IsRequired();

            entity.Property(p => p.FechaFin)
                  .IsRequired();

            entity.Property(p => p.FechaDevolucion)
                  .IsRequired(false);

            entity.Property(p => p.Estado)
                  .HasColumnName("Estado")
                  .HasConversion<string>()
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(p => p.EstaActivo)
                  .HasColumnName("EstaActiva")
                  .IsRequired();

            // Relaciones

            entity.HasOne<Libro>()
                  .WithMany()
                  .HasForeignKey(p => p.ISBN)
                  .HasPrincipalKey(l => l.ISBN)
                  .OnDelete(DeleteBehavior.Restrict);  // ✅ buena práctica

            entity.HasOne<Usuario>()
                  .WithMany()
                  .HasForeignKey(p => p.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
