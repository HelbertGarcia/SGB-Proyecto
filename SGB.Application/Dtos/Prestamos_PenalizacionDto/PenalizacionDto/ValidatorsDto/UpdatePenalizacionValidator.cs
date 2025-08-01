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
                .GreaterThan(0)
                .WithMessage("El ID de la penalización debe ser mayor a cero.");


          

            RuleFor(x => x.Motivo)
               
                .MaximumLength(200).WithMessage("El motivo no debe superar los 200 caracteres.");

            RuleFor(x => x.FechaFin)
                
                .GreaterThan(DateTime.MinValue).WithMessage("La fecha de fin debe ser válida.");

          


            RuleFor(x => x.Monto)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");
        }
    }
}
