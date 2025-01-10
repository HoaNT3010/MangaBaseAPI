using MangaBaseAPI.Contracts.Chapters.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Chapters.Queries.GetById
{
    public record GetChapterByIdQuery(
        Guid Id) : IRequest<Result<GetChapterByIdResponse>>;
}
