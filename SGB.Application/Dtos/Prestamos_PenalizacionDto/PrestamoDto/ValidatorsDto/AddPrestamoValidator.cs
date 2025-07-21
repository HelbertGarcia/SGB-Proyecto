using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto.ValidatosDto
{
    public class AddPrestamoDtoValidator : AbstractValidator<AddPrestamoDto>
    {
        public AddPrestamoDtoValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a cero.");


            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("El ISBN no puede estar vacío.")
                .Length(13).WithMessage("El ISBN debe tener exactamente 13 caracteres.");
    


            RuleFor(x => x.FechaInicio)
                .LessThan(x => x.FechaFin)
                .WithMessage("La fecha de inicio debe ser menor que la fecha de vencimiento.");
        }
    }
    }


