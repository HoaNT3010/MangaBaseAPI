using MangaBaseAPI.Application.Common.Utilities.Paging;
using MangaBaseAPI.Contracts.Creators.Search;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Creators.Queries
{
    public record SearchCreatorByNameQuery(
        string Keyword,
        int Page = 1,
        int PageSize = 10) : IRequest<Result<PagedList<SearchCreatorByNameResponse>>>;
}
