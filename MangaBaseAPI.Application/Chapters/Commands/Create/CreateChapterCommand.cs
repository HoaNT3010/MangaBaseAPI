using MangaBaseAPI.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MangaBaseAPI.Application.Chapters.Commands.Create
{
    public record CreateChapterCommand(
        Guid TitleId,
        string Name,
        float Index,
        int Volume,
        IFormFileCollection ChapterImages,
        Guid UploaderId) : IRequest<Result>;
}
