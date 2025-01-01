using FluentValidation;
using MangaBaseAPI.Application.Tittles.Commands.Create;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateAlternativeNames
{
    public class UpdateTitleAlternativeNamesCommandValidator
        : AbstractValidator<UpdateTitleAlternativeNamesCommand>
    {
        public UpdateTitleAlternativeNamesCommandValidator()
        {
            RuleFor(x => x.AlternativeNames)
                .NotNull().WithMessage("Title's alternative names cannot be null")
                .Must(x => x.Count <= 50).WithMessage("Title cannot have more than 10 alternative names");

            RuleFor(x => x.AlternativeNames)
                .Must(x => x.Select(item => item.Name).Distinct().Count() == x.Count)
                .WithMessage("Each alternative name in the list must have a unique value");

            RuleForEach(x => x.AlternativeNames)
                .SetValidator(new TitleAlternativeNameValidator());
        }
    }
}
