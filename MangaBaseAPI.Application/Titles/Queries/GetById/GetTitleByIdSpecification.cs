using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Titles.Queries.GetById
{
    public class GetTitleByIdSpecification : Specification<Title>
    {
        public GetTitleByIdSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AsTracking = false;
            IsSplitQuery = true;
        }
    }
}
