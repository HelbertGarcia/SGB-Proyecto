using FluentValidation;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Api.Validators.FluentValidator.Configuracion
{
    public class DeleteConfiguracionDtoValidator : AbstractValidator<DisableConfiguracionDto>
    {
        public DeleteConfiguracionDtoValidator()
        {
            RuleFor(c => c.IDConfiguracion).GreaterThan(0).WithMessage("Debe proporcionar un ID válido para deshabilitar");

            RuleFor(c => c.IDConfiguracion).GreaterThan(0).WithMessage("El ID de configuración debe ser mayor a 0");
        }
    }
}