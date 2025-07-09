using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto.Validators
{
    public class DisablePenalizacionDtoValidator : AbstractValidator<DisablePenalizacionDto>
    {
        public DisablePenalizacionDtoValidator()
        {
            RuleFor(x => x.IDPenalizacion)
                .GreaterThan(0).WithMessage("El ID de la penalización debe ser mayor a cero.");
        }
    }
}
