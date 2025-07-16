using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto.Validators
{
    public class AddPenalizacionDtoValidator : AbstractValidator<AddPenalizacionDto>
    {
        public AddPenalizacionDtoValidator()
        {
            RuleFor(x => x.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio)
                .WithMessage("La fecha de fin debe ser posterior a la de inicio.");

            RuleFor(x => x.Monto)
                .NotNull().GreaterThan(0)
                .WithMessage("El monto debe ser mayor que cero.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("El usuario es obligatorio.");
        }
    }

}
