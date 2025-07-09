using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto.Validators
{
    public class UpdatePenalizacionDtoValidator : AbstractValidator<UpdatePenalizacionDto>
    {
        public UpdatePenalizacionDtoValidator()
        {
            RuleFor(x => x.IDPenalizacion)
                 .GreaterThan(0).WithMessage("El ID de la penalización debe ser mayor a cero.");

            RuleFor(x => x.Motivo)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Motivo))
                .WithMessage("El motivo no debe superar los 200 caracteres.");

            RuleFor(x => x.FechaInicio)
                .LessThan(x => x.FechaFin)
                .When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue)
                .WithMessage("La fecha de inicio debe ser menor que la fecha de fin.");

            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio)
                .When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue)
                .WithMessage("La fecha de fin debe ser mayor que la fecha de inicio.");

        }
    }
}
