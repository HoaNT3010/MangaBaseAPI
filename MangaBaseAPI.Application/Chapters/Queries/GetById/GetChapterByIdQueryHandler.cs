using AutoMapper;
using MangaBaseAPI.Contracts.Chapters.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Errors.Chapter;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Chapters.Queries.GetById
{
    public class GetChapterByIdQueryHandler
        : IRequestHandler<GetChapterByIdQuery, Result<GetChapterByIdResponse>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _cache;
        readonly IMapper _mapper;

        public GetChapterByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache cache,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<Result<GetChapterByIdResponse>> Handle(
            GetChapterByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cachedData = await _cache
                .GetStringAsync(ChapterCachingConstants.GetByIdKey + request.Id.ToString(), cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                var chapterRepository = _unitOfWork.GetRepository<IChapterRepository>();

                var chapter = await chapterRepository.FirstOrDefaultAsync(
                    chapterRepository.ApplySpecification(new GetChapterByIdSpecification(request.Id)));

                if (chapter == null)
                {
                    return Result.Failure<GetChapterByIdResponse>(Error.ErrorWithValue(
                        ChapterErrors.General_ChapterNotFound,
                        request.Id));
                }
                if (chapter.IsDeleted)
                {
                    return Result.Failure<GetChapterByIdResponse>(Error.ErrorWithValue(
                        ChapterErrors.General_ChapterDeleted,
                        request.Id));
                }

                var result = _mapper.Map<GetChapterByIdResponse>(chapter);
                string resultJsonString = JsonConvert.SerializeObject(result);
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                    SlidingExpiration = TimeSpan.FromHours(1),
                };

                await _cache.SetStringAsync(ChapterCachingConstants.GetByIdKey + request.Id.ToString(),
                    resultJsonString,
                    cacheOptions,
                    cancellationToken);

                return Result.SuccessNullError(result);
            }

            return Result.SuccessNullError(JsonConvert.DeserializeObject<GetChapterByIdResponse>(cachedData!))!;
        }
    }
}
