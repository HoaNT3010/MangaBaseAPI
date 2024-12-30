using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateAuthors
{
    internal class UpdateTitleAuthorsSpecification
        : Specification<Title>
    {
        public UpdateTitleAuthorsSpecification(
            Guid id) : base(x => x.Id == id)
        {
            AddInclude(x => x.TitleAuthors);
            AsTracking = true;
        }
    }
}
