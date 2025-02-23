using FluentValidation;

namespace MangaBaseAPI.Application.Users.Commands.UpdateUserName
{
    public class UpdateUserUserNameCommandValidator : AbstractValidator<UpdateUserUserNameCommand>
    {
        public UpdateUserUserNameCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User name cannot be empty")
                .MinimumLength(6).WithMessage("User name must be at least 6 characters")
                .MaximumLength(24).WithMessage("User name must be at most 24 characters")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("User name can only contains alphanumeric characters");
        }
    }
}
