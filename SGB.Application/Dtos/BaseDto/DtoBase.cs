using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SGB.Application.Dtos.BaseDto
{
    public abstract class DtoBase
    {
        public DataSetDateTime changeDate {get; set; }
        public string? changeUser { get; set; }
    }

}

