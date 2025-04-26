using FluentValidation;
using Domain.Entities;
namespace Application.Validations;
public class LivroValidator : AbstractValidator<Livro>
{
    public LivroValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(40)
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Não pode conter caracteres especiais.");

        RuleFor(x => x.Editora)
            .NotEmpty().WithMessage("A editora é obrigatória.")
            .MaximumLength(40);
            

        RuleFor(x => x.Edicao)
            .GreaterThan(0).WithMessage("A edição deve ser maior que zero.");
 
        RuleFor(x => x.AnoPublicacao)
            .NotEmpty().WithMessage("O ano de publicação é obrigatório.")
            .MaximumLength(4)
            .Matches(@"^\d{4}$").WithMessage("O ano de publicação deve ter 4 dígitos.")
            .Must(x => int.TryParse(x, out int ano) && ano >= 1900 && ano <= DateTime.Now.Year);

        RuleFor(x => x.Preco)
            .GreaterThanOrEqualTo(0).WithMessage("O preço deve ser maior ou igual a zero.");
    }
}
