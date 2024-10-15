using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Queries.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
