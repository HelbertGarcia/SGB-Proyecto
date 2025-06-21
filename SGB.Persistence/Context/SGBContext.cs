using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Entities.Usuario;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Notificaciones;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Entities.Rol;

namespace SGB.Persistence.Context
{
    public class SGBContext: DbContext
    {
        public SGBContext(DbContextOptions<SGBContext> options) : base(options)
        {

        }

        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Ejemplares { get; set; }
        public DbSet<Notificacion> Notificacion { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Penalizacion> Penalizacion { get; set; }
        public DbSet<Rol> Rol { get; set; }

    }
}
