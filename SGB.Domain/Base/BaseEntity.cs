using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity(int id)
        {
            Id = id;
        }
        protected BaseEntity() { }

        public int Id { get; private set; }
    }
}
