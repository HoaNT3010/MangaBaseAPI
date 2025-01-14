using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Creators.Queries.GetById
{
    internal class GetCreatorByIdSpecification : Specification<Creator>
    {
        public GetCreatorByIdSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AsTracking = false;
        }
    }
}
