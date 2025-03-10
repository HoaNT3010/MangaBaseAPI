﻿using AutoMapper;
using MangaBaseAPI.Contracts.Genres.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Genres.Queries.GetAll
{
    public class GetAllGenresQueryHandler
        : IRequestHandler<GetAllGenresQuery, Result<List<GenreResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;

        public GetAllGenresQueryHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GenreResponse>>> Handle(
            GetAllGenresQuery request,
            CancellationToken cancellationToken)
        {
            string? cachedData = await _distributedCache
                .GetStringAsync(GenreCachingConstants.GetAllKey, cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                var genresList = await _mapper
                    .ProjectTo<GenreResponse>(_unitOfWork.GetRepository<IGenreRepository>().GetQueryableSet())
                    .ToListAsync(cancellationToken);

                if (genresList is { Count: > 0 })
                {
                    string genresListString = JsonConvert.SerializeObject(genresList);

                    await _distributedCache.SetStringAsync(GenreCachingConstants.GetAllKey,
                       genresListString,
                       CachingOptionConstants.YearlyCachingOption,
                       cancellationToken);
                }

                return Result.SuccessNullError(genresList)!;
            }

            return Result.SuccessNullError(JsonConvert.DeserializeObject<List<GenreResponse>>(cachedData!))!;
        }
    }
}
