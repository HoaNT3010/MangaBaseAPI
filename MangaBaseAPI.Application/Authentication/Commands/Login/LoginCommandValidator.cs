using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
