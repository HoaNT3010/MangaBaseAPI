using MangaBaseAPI.Application.Common.Utilities.Paging;
using MangaBaseAPI.Contracts.Chapters.GetChaptersByTitleId;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Chapters.Queries.GetChaptersByTitleId
{
    public record GetChaptersByTitleIdQuery(
        Guid Id,
        int Page = 1,
        int PageSize = 50,
        bool DescendingIndexOrder = true) : IRequest<Result<PagedList<GetChaptersByTitleIdResponse>>>;
}
