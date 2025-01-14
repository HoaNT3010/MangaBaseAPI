using AutoMapper;
using MangaBaseAPI.Contracts.LanguageCodes.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.LanguageCodes.Queries.GetAll
{
    public class GetAllLanguageCodesQueryHandler
        : IRequestHandler<
            GetAllLanguageCodesQuery,
            Result<List<LanguageCodeResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;

        public GetAllLanguageCodesQueryHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache,
            IMapper mapper)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<LanguageCodeResponse>>> Handle(
            GetAllLanguageCodesQuery request,
            CancellationToken cancellationToken)
        {
            var cachedData = await _distributedCache.GetStringAsync(
                LanguageCodeCachingConstants.GetAllKey,
                cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                var languagesList = await _mapper
                    .ProjectTo<LanguageCodeResponse>(_unitOfWork.GetRepository<ILanguageCodeRepository>().GetQueryableSet())
                    .ToListAsync(cancellationToken);

                if (languagesList is { Count: > 0 })
                {
                    string languagesListString = JsonConvert.SerializeObject(languagesList);

                    await _distributedCache.SetStringAsync(
                        LanguageCodeCachingConstants.GetAllKey,
                        languagesListString,
                        CachingOptionConstants.YearlyCachingOption,
                        cancellationToken);
                }

                return Result.SuccessNullError(languagesList)!;
            }

            return Result.SuccessNullError(JsonConvert.DeserializeObject<List<LanguageCodeResponse>>(cachedData!))!;
        }
    }
}
