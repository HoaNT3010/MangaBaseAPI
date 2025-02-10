using FluentValidation;

namespace MangaBaseAPI.Application.Creators.Queries.SearchByName
{
    public class SearchCreatorByNameQueryValidator : AbstractValidator<SearchCreatorByNameQuery>
    {
        public SearchCreatorByNameQueryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Keyword)
                .NotNull().WithMessage("Keyword is required")
                .MaximumLength(100).WithMessage("Keyword cannot exceed 100 characters");

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50");
        }
    }
}
