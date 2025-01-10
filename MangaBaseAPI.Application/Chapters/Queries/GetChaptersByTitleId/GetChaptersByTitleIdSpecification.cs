using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.GetChaptersByTitleId
{
    internal class GetChaptersByTitleIdSpecification : Specification<Chapter>
    {
        public GetChaptersByTitleIdSpecification(Guid id, bool descendingIndexOrder)
            : base(x => x.TitleId == id && !x.IsDeleted)
        {
            AsTracking = false;

            if (descendingIndexOrder)
            {
                AddOrderByDescending(x => x.Index);
            }
            else
            {
                AddOrderBy(x => x.Index);
            }
        }
    }
}
