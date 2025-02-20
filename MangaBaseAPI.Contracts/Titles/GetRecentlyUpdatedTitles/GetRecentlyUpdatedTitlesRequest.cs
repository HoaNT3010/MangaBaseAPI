using System.Reflection;
using MangaBaseAPI.Contracts.Common;
using Microsoft.AspNetCore.Http;

namespace MangaBaseAPI.Contracts.Titles.GetRecentlyUpdatedTitles
{
    public record GetRecentlyUpdatedTitlesRequest(
        int Page = 1,
        int PageSize = 10) : IExtensionBinder<GetRecentlyUpdatedTitlesRequest>
    {
        public static ValueTask<GetRecentlyUpdatedTitlesRequest?> BindAsync(
            HttpContext httpContext,
            ParameterInfo parameter)
        {
            if (!int.TryParse(httpContext.Request.Query["Page"], out var page))
            {
                page = 1;
            }
            if (!int.TryParse(httpContext.Request.Query["PageSize"], out var pageSize))
            {
                pageSize = 10;
            }

            var result = new GetRecentlyUpdatedTitlesRequest(page, pageSize);

            return ValueTask.FromResult<GetRecentlyUpdatedTitlesRequest?>(result);
        }
    }
}
