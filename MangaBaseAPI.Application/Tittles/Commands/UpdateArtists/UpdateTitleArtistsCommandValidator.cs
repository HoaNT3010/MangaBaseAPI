using FluentValidation;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateArtists
{
    public class UpdateTitleArtistsCommandValidator : AbstractValidator<UpdateTitleArtistsCommand>
    {
        public UpdateTitleArtistsCommandValidator()
        {
            RuleFor(x => x.Artists)
                .NotNull().WithMessage("Title's artists cannot be null")
                .Must(x => x.Distinct().Count() == x.Count).WithMessage("Title's artists cannot contains duplicate(s)")
                .Must(x => x.Count <= 10).WithMessage("Title cannot have more than 10 artists");
        }
    }
}
