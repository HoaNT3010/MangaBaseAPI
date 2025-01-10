using MangaBaseAPI.Contracts.Common;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace MangaBaseAPI.Contracts.Chapters.GetChaptersByTitleId
{
    public record GetChaptersByTitleIdRequest(
        int Page = 1,
        int PageSize = 50,
        bool DescendingIndexOrder = true) : IExtensionBinder<GetChaptersByTitleIdRequest>
    {
        public static ValueTask<GetChaptersByTitleIdRequest?> BindAsync(
            HttpContext httpContext,
            ParameterInfo parameter)
        {
            if (!int.TryParse(httpContext.Request.Query["Page"], out var page))
            {
                page = 1;
            }
            if (!int.TryParse(httpContext.Request.Query["PageSize"], out var pageSize))
            {
                pageSize = 50;
            }
            if (!bool.TryParse(httpContext.Request.Query["DescendingIndexOrder"], out var descendingIndexOrder))
            {
                descendingIndexOrder = true;
            }

            var result = new GetChaptersByTitleIdRequest(page, pageSize, descendingIndexOrder);

            return ValueTask.FromResult<GetChaptersByTitleIdRequest?>(result);
        }
    }
}
