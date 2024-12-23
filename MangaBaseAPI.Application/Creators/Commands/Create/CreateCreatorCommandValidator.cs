using FluentValidation;

namespace MangaBaseAPI.Application.Creators.Commands.Create
{
    public class CreateCreatorCommandValidator : AbstractValidator<CreateCreatorCommand>
    {
        public CreateCreatorCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Creator name cannot be empty")
                .MaximumLength(100).WithMessage("Creator name cannot exceed 100 characters");

            RuleFor(x => x.Biography)
                .NotEmpty()
                .WithMessage("Creator biography cannot be empty")
                .MaximumLength(1000)
                .WithMessage("Creator biography cannot exceed 1000 characters")
                .When(x => x.Biography != null);
        }
    }
}
