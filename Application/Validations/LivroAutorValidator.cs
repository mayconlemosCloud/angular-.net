using FluentValidation;
using Domain.Entities;
namespace Application.Validations;
public class LivroAutorValidator : AbstractValidator<LivroAutor>
{
    public LivroAutorValidator()
    {
        RuleFor(x => x.LivroCodl)
            .GreaterThan(0).WithMessage("O código do livro deve ser maior que zero.");

        RuleFor(x => x.AutorCodAu)
            .GreaterThan(0).WithMessage("O código do autor deve ser maior que zero.");
    }
}
