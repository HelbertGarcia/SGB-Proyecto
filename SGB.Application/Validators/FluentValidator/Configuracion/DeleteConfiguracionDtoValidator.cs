using FluentValidation;
using SGB.Application.Dtos.ConfiguracionDto;

namespace SGB.Application.Validators.FluentValidator.Configuracion
{
    public class DeleteConfiguracionDtoValidator : AbstractValidator<DisableConfiguracionDto>
    {
        public DeleteConfiguracionDtoValidator()
        {
            RuleFor(c => c.IDConfiguracion).GreaterThan(0).WithMessage("Debe proporcionar un ID válido para eliminar");

            RuleFor(c => c.IDConfiguracion).GreaterThan(0).WithMessage("El ID de configuración debe ser mayor a 0");
        }
    }
}