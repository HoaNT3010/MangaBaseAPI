using FluentValidation;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAuthors
{
    public class UpdateTitleAuthorsCommandValidator : AbstractValidator<UpdateTitleAuthorsCommand>
    {
        public UpdateTitleAuthorsCommandValidator()
        {
            RuleFor(x => x.Authors)
                .NotNull().WithMessage("Title's authors cannot be null")
                .Must(x => x.Distinct().Count() == x.Count).WithMessage("Title's authors cannot contains duplicate(s)")
                .Must(x => x.Count <= 10).WithMessage("Title cannot have more than 10 authors");
        }
    }
}
