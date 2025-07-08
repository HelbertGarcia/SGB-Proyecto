using FluentValidation;
using SGB.Application.Dtos.AdministracionDto;

namespace SGB.Application.Validators.FluentValidator.Configuracion
{
    public class GetConfiguracionDtoValidator : AbstractValidator<GetConfiguracionDto>
    {
        public GetConfiguracionDtoValidator()
        {
            RuleFor(c => c.IDConfiguracion).GreaterThan(0).WithMessage("El ID de configuración debe ser mayor a 0");

            RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacío")
           .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres");

            RuleFor(c => c.FechaCreacion).NotEmpty().WithMessage("La fecha de creación es requerida");

            RuleFor(c => c.EstaActivo).NotNull().WithMessage("Debe indicar si la configuración está activa o no");
        }
    }
}
