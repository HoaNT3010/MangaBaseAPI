using MangaBaseAPI.Application.Common.Utilities.Paging;
using MangaBaseAPI.Contracts.Titles.Common;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Queries.GetRecentlyUpdatedTitles
{
    public record GetRecentlyUpdatedTitlesQuery(
        int Page,
        int PageSize) : IRequest<Result<PagedList<ShortTitleResponse>>>;
}
