using FluentValidation;
using MangaBaseAPI.Domain.Enums;

namespace MangaBaseAPI.Application.Titles.Commands.Create
{
    public class CreateTitleCommandValidator : AbstractValidator<CreateTitleCommand>
    {
        public CreateTitleCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title name cannot be empty")
                .MaximumLength(250).WithMessage("Title name cannot exceed 250 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => x.Description != null)
                .WithMessage("Title description cannot exceed 1000 characters");

            RuleFor(x => x.TitleType)
                .Must(x => Enum.IsDefined(typeof(TitleType), x))
                .WithMessage("Title type must be a valid value");

            RuleFor(x => x.TitleStatus)
                .Must(x => Enum.IsDefined(typeof(TitleStatus), x))
                .WithMessage("Title status must be a valid value");

            RuleFor(x => x.PublishedDate)
                .GreaterThanOrEqualTo(new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .When(x => x.PublishedDate.HasValue)
                .WithMessage("Title publish date must be later than year 1900");

            RuleFor(x => x.Genres)
                .Must(x => x == null || x.Count <= 50).WithMessage("Title cannot have more than 50 genres")
                .Must(x => x == null || x.Distinct().Count() == x.Count).WithMessage("Title's genres cannot contains duplicate(s)");

            RuleFor(x => x.AlternativeNames)
                .Must(x => x == null || x.Count <= 50).WithMessage("Title cannot have more than 50 alternative names")
                .Must(x => x == null || x.Select(item => item.Name).Distinct().Count() == x.Count)
                .WithMessage("Title's alternative names cannot contains duplicate(s) with same name"); ;

            RuleFor(x => x.Authors)
                .Must(x => x == null || x.Count <= 10).WithMessage("Title cannot have more than 10 authors")
                .Must(x => x == null || x.Distinct().Count() == x.Count).WithMessage("Title's authors cannot contains duplicate(s)");

            RuleFor(x => x.Artists)
                .Must(x => x == null || x.Count <= 10).WithMessage("Title cannot have more than 10 artists")
                .Must(x => x == null || x.Distinct().Count() == x.Count).WithMessage("Title's artists cannot contains duplicate(s)");

            RuleForEach(x => x.AlternativeNames)
                .SetValidator(new TitleAlternativeNameValidator());
        }
    }

    public class TitleAlternativeNameValidator : AbstractValidator<TitleAlternativeName>
    {
        public TitleAlternativeNameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title alternative name cannot be empty")
                .MaximumLength(250).WithMessage("Title alternative name cannot exceed 250 characters");

            RuleFor(x => x.LanguageCodeId)
                .NotEmpty().WithMessage("Language code for alternative name cannot be empty")
                .MaximumLength(2).WithMessage("Language code for alternative name cannot exceed 2 characters");
        }
    }
}
