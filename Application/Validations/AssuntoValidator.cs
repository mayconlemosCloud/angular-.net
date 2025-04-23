using FluentValidation;
using Domain.Entities;


namespace Application.Validations;
public class AssuntoValidator : AbstractValidator<Assunto>
{
    public AssuntoValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição do assunto é obrigatória.")
            .MaximumLength(20);
    }
}
