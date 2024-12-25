using MangaBaseAPI.Contracts.Common;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace MangaBaseAPI.Contracts.Creators.Search
{
    public record SearchCreatorByNameRequest(
        string Keyword,
        int Page = 1,
        int PageSize = 10) : IExtensionBinder<SearchCreatorByNameRequest>
    {
        public static ValueTask<SearchCreatorByNameRequest?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
        {
            string keyword = httpContext.Request.Query["Keyword"];
            if (!int.TryParse(httpContext.Request.Query["Page"], out var page))
            {
                page = 1;
            }
            if (!int.TryParse(httpContext.Request.Query["PageSize"], out var pageSize))
            {
                pageSize = 10;
            }

            var result = new SearchCreatorByNameRequest(keyword, page, pageSize);

            return ValueTask.FromResult<SearchCreatorByNameRequest?>(result);
        }
    };
}
