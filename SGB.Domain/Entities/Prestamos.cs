using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Domain.Entities
{
    public class Prestamos : BaseEntity
    {
        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public DateTime fechaDevolucion { get; set; }
        public string estadoPrestamo { get; set; }



        public Prestamos(int id, DateTime FechaPrestamo, DateTime FechaVencimiento, DateTime FechaDevolucion, string EstadoPrestamo) : base(id)
        {
            //Los demas conectados
            fechaPrestamo = FechaPrestamo;
            fechaVencimiento = FechaVencimiento;
            fechaDevolucion = FechaDevolucion;
            estadoPrestamo = EstadoPrestamo;
        }


    }
}