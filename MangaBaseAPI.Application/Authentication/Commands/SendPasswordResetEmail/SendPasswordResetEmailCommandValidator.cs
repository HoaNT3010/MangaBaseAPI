using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Commands.SendPasswordResetEmail
{
    public class SendPasswordResetEmailCommandValidator : AbstractValidator<SendPasswordResetEmailCommand>
    {
        public SendPasswordResetEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email address cannot be null")
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");
        }
    }
}
