using FluentValidation;
using MangaBaseAPI.Application.Titles.Commands.Create;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAlternativeNames
{
    public class UpdateTitleAlternativeNamesCommandValidator
        : AbstractValidator<UpdateTitleAlternativeNamesCommand>
    {
        public UpdateTitleAlternativeNamesCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.AlternativeNames)
                .NotNull().WithMessage("Title's alternative names cannot be null")
                .Must(x => x == null || x.Count <= 50).WithMessage("Title cannot have more than 10 alternative names")
                .Must(x => x == null || x.Select(item => item.Name).Distinct().Count() == x.Count).WithMessage("Each alternative name in the list must have a unique value");

            RuleForEach(x => x.AlternativeNames)
                .SetValidator(new TitleAlternativeNameValidator());
        }
    }
}
