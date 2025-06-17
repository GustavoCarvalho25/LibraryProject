using FluentValidation;

namespace Application.Queries.Books.GetBookByIdQuery;

public class GetBookByIdValidation : AbstractValidator<GetBookByIdQuery>
{
    public GetBookByIdValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The book ID must be provided.")
            .NotNull().WithMessage("The book ID cannot be null.")
            .Must(x => x != Guid.Empty).WithMessage("The book ID cannot be an empty GUID.")
            .Must(x => x is Guid).WithMessage("The book ID must be of type Guid.");
    }
}