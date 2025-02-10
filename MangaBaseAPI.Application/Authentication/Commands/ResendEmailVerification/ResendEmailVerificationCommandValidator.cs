using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Commands.ResendEmailVerification
{
    public class ResendEmailVerificationCommandValidator : AbstractValidator<ResendEmailVerificationCommand>
    {
        public ResendEmailVerificationCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email address cannot be null")
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");
        }
    }
}
