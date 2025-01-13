using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Chapters.Commands.Delete
{
    public record DeleteChapterCommand(
        Guid Id) : IRequest<Result>;
}
