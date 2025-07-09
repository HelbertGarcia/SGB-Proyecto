using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatosDto
{
    public class DisablePrestamoDtoValidator : AbstractValidator<DiseblePrestamoDto>
    {
        public DisablePrestamoDtoValidator()
        {
            RuleFor(x => x.IDPrestamo)
                .GreaterThan(0).WithMessage("El ID del préstamo debe ser mayor a cero.");
        }
    }
}
