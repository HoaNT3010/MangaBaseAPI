using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateGenres
{
    internal class UpdateTitleGenresSpecification : Specification<Title>
    {
        public UpdateTitleGenresSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.TitleGenres);
            AsTracking = true;
        }
    }
}
