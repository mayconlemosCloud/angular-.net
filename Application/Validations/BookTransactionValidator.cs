using FluentValidation;
using Domain.Entities;

namespace Application.Validations
{
    public class BookTransactionValidator : AbstractValidator<BookTransaction>
    {
        public BookTransactionValidator()
        {
            RuleFor(x => x.LivroCodl)
                .GreaterThan(0).WithMessage("O código do livro deve ser maior que zero.");

            RuleFor(x => x.FormaDeCompra)
                .NotEmpty().WithMessage("A forma de compra não pode ser vazia.");            
        }
    }
}
