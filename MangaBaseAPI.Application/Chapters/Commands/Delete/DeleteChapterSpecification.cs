using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Commands.Delete
{
    internal class DeleteChapterSpecification : Specification<Chapter>
    {
        public DeleteChapterSpecification(Guid id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ChapterImages);
        }
    }
}
