using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatosDto
{
    public class UpdatePrestamoDtoValidator : AbstractValidator<UpdatePrestamoDto>
    {
        public UpdatePrestamoDtoValidator()
        {
            RuleFor(x => x.IDPrestamo)
                 .GreaterThan(0).WithMessage("El ID del préstamo debe ser mayor a cero.");

            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio.Value)
                .When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue)
                .WithMessage("La fecha de vencimiento debe ser posterior a la fecha de inicio.");

            RuleFor(x => x.Estado)
                .MaximumLength(50).WithMessage("El estado no debe superar los 50 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Estado));
        }
    }
}
