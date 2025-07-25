using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SGB.Presentation.Models.Libro
{
    public class LibroCreateViewModel
    {
        public LibroModel Libro { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoriasDisponibles { get; set; }
    }
}