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
            RuleFor(x => x.UsuarioId)
                     .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a cero.");

            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("El motivo no puede estar vacío.")
                .MaximumLength(200).WithMessage("El motivo no debe superar los 200 caracteres.");

            RuleFor(x => x.FechaInicio)
                .LessThan(x => x.FechaFin)
                .WithMessage("La fecha de inicio debe ser menor que la fecha de fin.");

            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio)
                .WithMessage("La fecha de fin debe ser mayor que la fecha de inicio.");
        }
    }

}
