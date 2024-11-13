using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User name cannot be empty")
                .MinimumLength(6).WithMessage("User name must be at least 6 characters")
                .MaximumLength(24).WithMessage("User name must be at most 24 characters")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("User name can only contains alphanumeric characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(24).WithMessage("Password must be at most 24 characters");

            RuleFor(x => x.ConfirmedPassword)
                .NotEmpty().WithMessage("Confirmed password cannot be empty")
                .Equal(x => x.Password).WithMessage("Confirmed password does not match password");
        }
    }
}
