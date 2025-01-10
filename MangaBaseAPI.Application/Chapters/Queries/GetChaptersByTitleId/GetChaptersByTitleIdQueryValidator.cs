using FluentValidation;

namespace MangaBaseAPI.Application.Chapters.Queries.GetChaptersByTitleId
{
    public class GetChaptersByTitleIdQueryValidator : AbstractValidator<GetChaptersByTitleIdQuery>
    {
        public GetChaptersByTitleIdQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");
        }
    }
}
