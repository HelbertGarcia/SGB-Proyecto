using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities
{
    public class Categoria:BaseEntity
    {
        public String nombre {  get; set; }

       public Categoria(string nombre) 
        {
            this.nombre = nombre;

        }




    }
}
