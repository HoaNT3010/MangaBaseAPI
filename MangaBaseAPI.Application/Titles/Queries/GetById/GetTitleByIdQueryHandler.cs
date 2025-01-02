using AutoMapper;
using MangaBaseAPI.Contracts.Titles.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Titles.Queries.GetById
{
    public class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, Result<GetTitleByIdResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public GetTitleByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public async Task<Result<GetTitleByIdResponse>> Handle(
            GetTitleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cachedData = await _distributedCache
                .GetStringAsync(TitleCachingConstants.GetByIdKey + request.Id.ToString(), cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
                var query = titleRepository.ApplySpecification(new GetTitleByIdSpecification(request.Id))
                    .Include(x => x.TitleGenres)
                        .ThenInclude(x => x.Genre)
                    .Include(x => x.AlternativeNames)
                        .ThenInclude(x => x.LanguageCode)
                    .Include(x => x.TitleAuthors)
                        .ThenInclude(x => x.Author)
                    .Include(x => x.TitleArtists)
                        .ThenInclude(x => x.Artist);

                var title = await titleRepository.FirstOrDefaultAsync(query);

                if (title == null)
                {
                    return Result.Failure<GetTitleByIdResponse>(TitleErrors.General_TitleNotFound);
                }

                var result = _mapper.Map<GetTitleByIdResponse>(title);

                string resultJsonString = JsonConvert.SerializeObject(result);
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                    SlidingExpiration = TimeSpan.FromHours(1),
                };

                await _distributedCache.SetStringAsync(TitleCachingConstants.GetByIdKey + request.Id.ToString(),
                    resultJsonString,
                    cacheOptions,
                    cancellationToken);

                return Result.SuccessNullError(result);
            }

            return Result.SuccessNullError(JsonConvert.DeserializeObject<GetTitleByIdResponse>(cachedData!))!;
        }
    }
}
