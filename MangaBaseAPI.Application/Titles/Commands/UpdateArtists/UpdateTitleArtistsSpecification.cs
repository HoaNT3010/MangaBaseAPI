using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateArtists
{
    internal class UpdateTitleArtistsSpecification : Specification<Title>
    {
        public UpdateTitleArtistsSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.TitleArtists);
            AsTracking = true;
        }
    }
}
