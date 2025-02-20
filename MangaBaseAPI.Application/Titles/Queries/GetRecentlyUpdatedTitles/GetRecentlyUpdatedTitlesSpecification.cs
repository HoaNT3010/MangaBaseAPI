using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Titles.Queries.GetRecentlyUpdatedTitles
{
    internal class GetRecentlyUpdatedTitlesSpecification : Specification<Title>
    {
        public GetRecentlyUpdatedTitlesSpecification()
            : base(t => t.Chapters.Count > 0 && !t.IsDeleted && !t.IsHidden)
        {
            //AddInclude(t => t.Chapters.OrderByDescending(c => c.Index).Take(1));
            AddOrderByDescending(t => t.Chapters.Max(c => c.CreatedDateTime));
            AsTracking = false;
        }
    }
}
