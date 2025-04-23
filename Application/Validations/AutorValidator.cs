using FluentValidation;
using Domain.Entities;
namespace Application.Validations;
public class AutorValidator : AbstractValidator<Autor>
{
    public AutorValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do autor é obrigatório.")
            .MaximumLength(40)
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Não pode conter caracteres especiais.");
    }
}
