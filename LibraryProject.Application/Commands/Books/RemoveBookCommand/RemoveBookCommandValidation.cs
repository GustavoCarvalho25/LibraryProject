using FluentValidation;

namespace Application.Commands.Books.RemoveBookCommand;

public class RemoveBookCommandValidation : AbstractValidator<RemoveBookCommand>
{
    public RemoveBookCommandValidation()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Book Id is required.");
    }
}
