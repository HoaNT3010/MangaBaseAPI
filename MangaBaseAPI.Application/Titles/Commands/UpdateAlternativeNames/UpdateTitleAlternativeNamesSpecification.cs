using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAlternativeNames
{
    internal class UpdateTitleAlternativeNamesSpecification : Specification<Title>
    {
        public UpdateTitleAlternativeNamesSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.AlternativeNames);
            AsTracking = true;
        }
    }
}
