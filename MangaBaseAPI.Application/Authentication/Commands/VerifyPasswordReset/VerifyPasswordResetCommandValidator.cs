using FluentValidation;

namespace MangaBaseAPI.Application.Authentication.Commands.VerifyPasswordReset
{
    public class VerifyPasswordResetCommandValidator : AbstractValidator<VerifyPasswordResetCommand>
    {
        public VerifyPasswordResetCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email address cannot be null")
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Email address is not valid");

            RuleFor(x => x.Token)
                .NotNull().WithMessage("Verification token cannot be null")
                .NotEmpty().WithMessage("Verification token cannot be empty")
                .Must(x => x == null || Guid.TryParse(x, out Guid result)).WithMessage("Invalid verification token format");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password cannot be empty")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters")
                .MaximumLength(24).WithMessage("New password must be at most 24 characters");

            RuleFor(x => x.ConfirmNewPassword)
                .NotNull().WithMessage("Confirm new password cannot be null")
                .NotEmpty().WithMessage("Confirm new password cannot be empty")
                .Must((model, x) => x == model.NewPassword)
                .WithMessage("Confirm new password must be the same as new password");
        }
    }
}
