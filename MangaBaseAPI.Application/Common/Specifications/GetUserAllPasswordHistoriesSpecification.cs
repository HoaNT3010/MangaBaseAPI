using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Common.Specifications
{
    internal class GetUserAllPasswordHistoriesSpecification : Specification<PasswordHistory>
    {
        public GetUserAllPasswordHistoriesSpecification(Guid userId)
            : base(x => x.UserId == userId)
        {
            AsTracking = false;
        }
    }
}
