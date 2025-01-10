using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.GetTitleChaptersList
{
    internal class GetTitleChaptersListSpecification : Specification<Chapter>
    {
        public GetTitleChaptersListSpecification(Guid id)
            : base(x => x.TitleId == id && !x.IsDeleted)
        {
            AsTracking = false;
            AddOrderBy(x => x.Index);
        }
    }
}
