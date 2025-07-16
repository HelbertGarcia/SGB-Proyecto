using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Entities.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Persistence.Context.Configurations
{
    public class PenalizacionConfiguration : IEntityTypeConfiguration<Penalizacion>
    {
        public void Configure(EntityTypeBuilder<Penalizacion> entity)
        {
            entity.ToTable("Penalizaciones");

            // Clave primaria
            entity.HasKey(p => p.Id);

            // Mapeo de columnas
            entity.Property(p => p.Id)
                  .HasColumnName("IDPenalizacion");

            entity.Property(p => p.IDUsuario)
                  .HasColumnName("IDUsuario")
                  .IsRequired();

            entity.Property(p => p.Motivo)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(p => p.FechaInicio)
                  .IsRequired();

            entity.Property(p => p.FechaFin)
                  .IsRequired();

        
            entity.Property(p => p.Monto)
                  .HasColumnType("decimal(10,2)")
                  .IsRequired(); 

            entity.Property(p => p.EstaActivo)
                  .HasColumnName("EstaActiva")
                  .IsRequired();

            entity.Property(p => p.FechaCreacion)
                  .IsRequired();

            entity.Property(p => p.FechaActualizacion)
                  .IsRequired();

            entity.Property(p => p.IDPrestamo)
                .IsRequired();


            // Relaciones
            entity.HasOne<Usuario>()
                  .WithMany()
                  .HasForeignKey(p => p.IDUsuario)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Prestamo>()
                 .WithMany()
                 .HasForeignKey(p => p.IDPrestamo)
                 .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
