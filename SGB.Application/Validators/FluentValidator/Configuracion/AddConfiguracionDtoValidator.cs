using FluentValidation;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Api.Validators.FluentValidator.Configuracion
{
    public class AddConfiguracionDtoValidator : AbstractValidator<AddConfiguracionDto>
    {
        public AddConfiguracionDtoValidator()
        {
            RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre de configuración no puede estar vacío")
           .MaximumLength(100).WithMessage("El nombre de configuración no debe exceder los 100 caracteres");

            RuleFor(c => c.Valor).NotEmpty().WithMessage("El valor de configuración no puede estar vacío");

            RuleFor(c => c.Descripcion).MaximumLength(255).WithMessage("La descripción no debe superar los 255 caracteres")
            .When(c => !string.IsNullOrWhiteSpace(c.Descripcion));
        }
    }
}
