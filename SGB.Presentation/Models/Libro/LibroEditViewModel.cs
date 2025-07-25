using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SGB.Presentation.Models.Libro
{
    public class LibroEditViewModel
    {
        public LibroModel Libro { get; set; }

        // --- CORRECCIÓN APLICADA AQUÍ ---
        // Con [ValidateNever], le decimos al sistema que no intente validar esta propiedad
        // cuando el formulario se envía, ya que no es un dato de entrada.
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoriasDisponibles { get; set; }
    }
}