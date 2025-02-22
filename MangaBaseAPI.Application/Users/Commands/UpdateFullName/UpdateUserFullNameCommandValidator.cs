using FluentValidation;

namespace MangaBaseAPI.Application.Users.Commands.UpdateFullName
{
    public class UpdateUserFullNameCommandValidator : AbstractValidator<UpdateUserFullNameCommand>
    {
        public UpdateUserFullNameCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty")
                .Matches(@"^[\p{L} \-]+$").WithMessage("Only first name with letters (including international characters), spaces and hyphens are valid")
                .MinimumLength(1).WithMessage("First name must be at least 1 character")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty")
                .Matches(@"^[\p{L} \-]+$").WithMessage("Only last name with letters (including international characters), spaces and hyphens are valid")
                .MinimumLength(1).WithMessage("Last name must be at least 1 character")
                .MaximumLength(100).WithMessage("Last name must be at most 100 characters");
        }
    }
}
