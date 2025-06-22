using FluentValidation;

namespace Application.Commands.Users.AddUserCommand;

public class AddUserCommandValidation : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do usuário é obrigatório")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres");
    }
}
