using AutoMapper;
using AutoMapper.QueryableExtensions;
using MangaBaseAPI.Application.Common.Utilities.Paging;
using MangaBaseAPI.Contracts.Titles.Common;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Titles.Queries.GetRecentlyUpdatedTitles
{
    public class GetRecentlyUpdatedTitlesQueryHandler(
        IUnitOfWork unitOfWork,
        IDistributedCache distributedCache,
        IMapper mapper)
        : IRequestHandler<GetRecentlyUpdatedTitlesQuery, Result<PagedList<ShortTitleResponse>>>
    {
        const int MaxCachedTitles = 100;

        public async Task<Result<PagedList<ShortTitleResponse>>> Handle(
            GetRecentlyUpdatedTitlesQuery request,
            CancellationToken cancellationToken)
        {
            // Apply caching + database query approach
            // Store 100 recently updated titles in cache
            // If user navigates passed the first 100 titles, fetch the additional titles from the database
            var takeResultFromCache = (request.Page - 1) * request.PageSize + request.PageSize <= MaxCachedTitles;
            var titleRepository = unitOfWork.GetRepository<ITitleRepository>();
            if (takeResultFromCache)
            {
                var cachedShortTitles = await distributedCache
                    .GetStringAsync(TitleCachingConstants.GetRecentlyUpdatedTitlesKey, cancellationToken);
                var cachedTotalCount = await distributedCache
                    .GetStringAsync(TitleCachingConstants.GetRecentlyUpdatedTitlesTotalCountKey, cancellationToken);

                if (!string.IsNullOrEmpty(cachedShortTitles) && int.TryParse(cachedTotalCount, out int totalCount))
                {
                    var cachedTitles = JsonConvert.DeserializeObject<List<ShortTitleResponse>>(cachedShortTitles)!.AsQueryable();
                    var result = PagedList<ShortTitleResponse>.Create(cachedTitles,
                        request.Page,
                        request.PageSize);
                    result.TotalCount = totalCount;
                    return Result.SuccessNullError(result);
                }

                var cacheQueryable = GetFilterQueryable(titleRepository, mapper.ConfigurationProvider);
                var fetchedTotalCount = await cacheQueryable.CountAsync(cancellationToken);
                var fetchedShortTitles = await cacheQueryable
                    .Take(MaxCachedTitles)
                    .ToListAsync(cancellationToken);

                await distributedCache.SetStringAsync(TitleCachingConstants.GetRecentlyUpdatedTitlesKey,
                    JsonConvert.SerializeObject(fetchedShortTitles),
                    CachingOptionConstants.UltraShortCachingOption,
                    cancellationToken);
                await distributedCache.SetStringAsync(TitleCachingConstants.GetRecentlyUpdatedTitlesTotalCountKey,
                    fetchedTotalCount.ToString(),
                    CachingOptionConstants.UltraShortCachingOption,
                    cancellationToken);
                var cacheQueryResult = PagedList<ShortTitleResponse>.Create(fetchedShortTitles.AsQueryable(),
                    request.Page,
                    request.PageSize);
                cacheQueryResult.TotalCount = fetchedTotalCount;
                return Result.SuccessNullError(cacheQueryResult);
            }

            var nonCacheQueryable = GetFilterQueryable(titleRepository, mapper.ConfigurationProvider);
            return Result.SuccessNullError(await PagedList<ShortTitleResponse>.CreateAsync(nonCacheQueryable, request.Page, request.PageSize, cancellationToken));
        }

        private IQueryable<ShortTitleResponse> GetFilterQueryable(ITitleRepository titleRepository, IConfigurationProvider configuration)
        {
            return titleRepository.ApplySpecification(new GetRecentlyUpdatedTitlesSpecification())
                .ProjectTo<ShortTitleResponse>(configuration);
        }
    }
}
