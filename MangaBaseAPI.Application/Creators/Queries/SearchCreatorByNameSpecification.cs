using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Creators.Queries
{
    public class SearchCreatorByNameSpecification : Specification<Creator>
    {
        public SearchCreatorByNameSpecification(string keyword)
            : base(x => string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword))
        {
            AsTracking = false;
            AddOrderBy(x => x.Name);
        }
    }
}
