using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatorsDto
{
    public class RegistrarDevolucionValidatorDto : AbstractValidator<RegistrarDevolucionDto>
    {
        public RegistrarDevolucionValidatorDto()
        {
            RuleFor(x => x.IdPrestamo)
                .GreaterThan(0).WithMessage("El ID del préstamo debe ser mayor a 0.");

            RuleFor(x => x.FechaDevolucion)
                .NotEmpty().WithMessage("La fecha de devolución es obligatoria.");
        }

    }
}
