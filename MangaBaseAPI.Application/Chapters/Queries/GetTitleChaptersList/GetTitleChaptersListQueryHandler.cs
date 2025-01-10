using AutoMapper;
using MangaBaseAPI.Contracts.Chapters.GetTitleChaptersList;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Chapters.Queries.GetTitleChaptersList
{
    public class GetTitleChaptersListQueryHandler
        : IRequestHandler<GetTitleChaptersListQuery, Result<List<GetTitleChaptersListResponse>>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IDistributedCache _cache;

        public GetTitleChaptersListQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetTitleChaptersListResponse>>> Handle(
            GetTitleChaptersListQuery request,
            CancellationToken cancellationToken)
        {
            var cachedData = await _cache.GetStringAsync(
                ChapterCachingConstants.GetTitleChaptersListConstant(request.Id),
                cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                var titleValidationError = await ValidateTitle(request.Id, _unitOfWork.GetRepository<ITitleRepository>());
                if (titleValidationError != null)
                {
                    return Result.Failure<List<GetTitleChaptersListResponse>>(titleValidationError);
                }

                var chapterRepository = _unitOfWork.GetRepository<IChapterRepository>();
                var chapters = await chapterRepository.ToListAsync(chapterRepository.ApplySpecification(new GetTitleChaptersListSpecification(request.Id)));
                var result = _mapper.Map<List<GetTitleChaptersListResponse>>(chapters);

                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7),
                    SlidingExpiration = TimeSpan.FromDays(1),
                };

                await _cache.SetStringAsync(
                    ChapterCachingConstants.GetTitleChaptersListConstant(request.Id),
                    JsonConvert.SerializeObject(result),
                    cacheOptions,
                    cancellationToken);

                return Result.SuccessNullError(result);
            }

            return Result.SuccessNullError(JsonConvert.DeserializeObject<List<GetTitleChaptersListResponse>>(cachedData!))!;
        }

        private async Task<Error?> ValidateTitle(Guid titleId,
            ITitleRepository titleRepository)
        {
            if (!await titleRepository.IsTitleExists(titleId))
            {
                return Error.ErrorWithValue(
                    TitleErrors.General_TitleNotFound,
                    titleId);
            }

            if (await titleRepository.IsTitleHidden(titleId))
            {
                return Error.ErrorWithValue(
                    TitleErrors.General_TitleHidden,
                    titleId);
            }

            if (await titleRepository.IsTitleDeleted(titleId))
            {
                return Error.ErrorWithValue(
                    TitleErrors.General_TitleDeleted,
                    titleId);
            }

            return Error.Null;
        }
    }
}
