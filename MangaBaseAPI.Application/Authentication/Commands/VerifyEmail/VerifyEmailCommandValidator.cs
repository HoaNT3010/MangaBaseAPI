using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email address cannot be null")
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");

            RuleFor(x => x.Token)
                .NotNull().WithMessage("Verification token cannot be null")
                .NotEmpty().WithMessage("Verification token cannot be empty")
                .Must(x => x == null || Guid.TryParse(x, out Guid result)).WithMessage("Invalid verification token format");
        }
    }
}
