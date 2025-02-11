using FluentValidation;

namespace MangaBaseAPI.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CurrentPassword)
                .NotNull().WithMessage("Current password cannot be null")
                .NotEmpty().WithMessage("Current password cannot be empty")
                .MinimumLength(6).WithMessage("Current password must be at least 6 characters")
                .MaximumLength(24).WithMessage("Current password must be at most 24 characters");

            RuleFor(x => x.NewPassword)
                .NotNull().WithMessage("New password cannot be null")
                .NotEmpty().WithMessage("New password cannot be empty")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters")
                .MaximumLength(24).WithMessage("New password must be at most 24 characters")
                .Must((model, x) => x != model.CurrentPassword)
                .WithMessage("New password cannot be the same as current password");

            RuleFor(x => x.ConfirmNewPassword)
                .NotNull().WithMessage("Confirm new password cannot be null")
                .NotEmpty().WithMessage("Confirm new password cannot be empty")
                .Must((model, x) => x == model.NewPassword)
                .WithMessage("Confirm new password must be the same as new password");
        }
    }
}
