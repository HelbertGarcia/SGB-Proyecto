using FluentValidation;
using SGB.Application.Dtos.ConfiguracionDto;

namespace SGB.Application.Validators.FluentValidator.Configuracion
{
    public class UpdateConfiguracionDtoValidator : AbstractValidator<UpdateConfiguracionDto>
    {
        public UpdateConfiguracionDtoValidator()
        {
            RuleFor(c => c.IDConfiguracion).GreaterThan(0).WithMessage("El ID de configuración debe ser mayor a 0");

            RuleFor(c => c.Nombre).MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres")
           .When(c => !string.IsNullOrWhiteSpace(c.Nombre));

            RuleFor(c => c.Valor).NotEmpty().WithMessage("El valor no puede estar vacío");

            RuleFor(c => c.Descripcion).MaximumLength(255).WithMessage("La descripción no debe superar los 255 caracteres")
           .When(c => c.Descripcion != null);

            RuleFor(c => c.EstaActivo).NotNull().WithMessage("Debe indicar si la configuración está activa o no");
        }
    }
}
