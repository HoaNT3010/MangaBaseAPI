using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.GetById
{
    public class GetChapterByIdSpecification : Specification<Chapter>
    {
        public GetChapterByIdSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ChapterImages);
            AddInclude(x => x.Title);
            AsTracking = false;
        }
    }
}
