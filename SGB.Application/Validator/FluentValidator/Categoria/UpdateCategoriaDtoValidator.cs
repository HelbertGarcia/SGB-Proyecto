using FluentValidation;
using SGB.Application.Dtos.LibrosDto.CategoriaDto; 

namespace SGB.Application.Validators.FluentValidators.Categoria
{
    public class UpdateCategoriaDtoValidator : AbstractValidator<UpdateCategoriaDto>
    {
        public UpdateCategoriaDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }
}