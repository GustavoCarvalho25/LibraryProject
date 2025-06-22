using FluentValidation;

namespace Application.Commands.Books;

public class ReturnBookCommandValidation : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidation()
    {
        RuleFor(command => command.BookId)
            .NotEmpty().WithMessage("Book Id is required.");
    }
}
