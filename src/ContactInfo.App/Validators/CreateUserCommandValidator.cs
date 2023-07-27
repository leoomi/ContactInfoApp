using System.Text.RegularExpressions;
using FluentValidation;
using ContactInfo.App.Models;
using ContactInfo.App.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("This field is required");
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Your password cannot be empty")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.");
        RuleFor(c => new {c.Password, c.PasswordConfirmation})
            .Must(c => c.Password == c.PasswordConfirmation).WithMessage("Password and password confirmation do not match.");
    }
}