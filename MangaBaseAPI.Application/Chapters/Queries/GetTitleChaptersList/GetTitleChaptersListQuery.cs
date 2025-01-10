using MangaBaseAPI.Contracts.Chapters.GetTitleChaptersList;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Chapters.Queries.GetTitleChaptersList
{
    public record GetTitleChaptersListQuery(
        Guid Id) : IRequest<Result<List<GetTitleChaptersListResponse>>>;
}
