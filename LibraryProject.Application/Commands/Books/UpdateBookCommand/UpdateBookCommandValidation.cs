using FluentValidation;

namespace Application.Commands.Books;

public class UpdateBookCommandValidation : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidation()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Book Id is required.");
            
        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(command => command.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(100).WithMessage("Author cannot exceed 100 characters.");

        RuleFor(command => command.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Matches(@"^\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$").WithMessage("ISBN must be in the format xxx-x-xxxx-xxxx-x.");

        RuleFor(command => command.PublicationYear)
            .InclusiveBetween(1450, DateTime.Now.Year).WithMessage("Publication year must be between 1450 and the current year.");
    }
}
