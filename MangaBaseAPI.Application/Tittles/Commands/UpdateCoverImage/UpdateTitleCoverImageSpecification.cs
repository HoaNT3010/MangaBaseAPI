using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateCoverImage
{
    public class UpdateTitleCoverImageSpecification : Specification<Title>
    {
        public UpdateTitleCoverImageSpecification(Guid id)
            : base(x => x.Id == id)
        {
        }
    }
}
