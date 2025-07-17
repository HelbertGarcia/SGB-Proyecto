using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;

namespace SGB.Application.validators.FluentValidators.Usuario
{
    public class DisableUsuarioDtoValidator : AbstractValidator<DisableUsuarioDto>
    {
        public DisableUsuarioDtoValidator()
        {
            RuleFor(x => x.IDUsuario)
                .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor que cero.");
        }
    }

}
