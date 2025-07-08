using FluentValidation;
using SGB.Application.Dtos.LibrosDto.LibroDto; 

namespace SGB.Application.Validators.FluentValidators.Libros
{
    public class AddLibroDtoValidator : AbstractValidator<AddLibroDto>
    {
        public AddLibroDtoValidator()
        {
            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("El ISBN es obligatorio.")
                .Length(10, 13).WithMessage("El ISBN debe tener 10 o 13 caracteres.");

            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");

            RuleFor(x => x.Autor)
                .NotEmpty().WithMessage("El autor es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre del autor no puede exceder los 150 caracteres.");

            RuleFor(x => x.IDCategoria)
                .GreaterThan(0).WithMessage("El ID de la categoría no es válido.");

            RuleFor(x => x.FechaPublicacion)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de publicación no puede ser una fecha futura.")
                .When(x => x.FechaPublicacion.HasValue);
        }
    }
}