using FluentValidation;

namespace Application.Commands.Books;

public class LoanBookCommandValidation : AbstractValidator<LoanBookCommand>
{
    public LoanBookCommandValidation()
    {
        RuleFor(command => command.BookId)
            .NotEmpty().WithMessage("Book Id is required.");
            
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User Id is required.");
    }
}
