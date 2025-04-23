using FluentValidation;
using Domain.Entities;
namespace Application.Validations;

public class LivroAssuntoValidator : AbstractValidator<LivroAssunto>
{
    public LivroAssuntoValidator()
    {
        RuleFor(x => x.LivroCodl)
            .GreaterThan(0).WithMessage("O código do livro deve ser maior que zero.");

        RuleFor(x => x.AssuntoCodAs)
            .GreaterThan(0).WithMessage("O código do assunto deve ser maior que zero.");
    }
}
