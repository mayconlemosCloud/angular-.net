using FluentValidation;
using Domain.Entities;
namespace Application.Validations;
public class LivroValidator : AbstractValidator<Livro>
{
    public LivroValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(40);

        RuleFor(x => x.Editora)
            .NotEmpty().WithMessage("A editora é obrigatória.")
            .MaximumLength(40);

        RuleFor(x => x.Edicao)
            .GreaterThan(0).WithMessage("A edição deve ser maior que zero.");

        RuleFor(x => x.AnoPublicacao)
            .NotEmpty().WithMessage("O ano de publicação é obrigatório.")
            .MaximumLength(4);
    }
}
