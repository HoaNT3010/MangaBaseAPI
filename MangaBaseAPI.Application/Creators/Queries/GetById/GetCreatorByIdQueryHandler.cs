using AutoMapper;
using MangaBaseAPI.Contracts.Creators.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Errors.Creator;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Creators.Queries.GetById
{
    public class GetCreatorByIdQueryHandler
        : IRequestHandler<GetCreatorByIdQuery, Result<GetCreatorByIdResponse>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _cache;
        readonly IMapper _mapper;

        public GetCreatorByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache cache,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<Result<GetCreatorByIdResponse>> Handle(
            GetCreatorByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cachedData = await _cache.GetStringAsync(
                CreatorCachingConstants.GetByIdKey + request.Id.ToString(),
                cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return Result.SuccessNullError(JsonConvert.DeserializeObject<GetCreatorByIdResponse>(cachedData!))!;
            }

            var creatorRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            var creator = await creatorRepository.FirstOrDefaultAsync(
                creatorRepository.ApplySpecification(new GetCreatorByIdSpecification(request.Id)),
                cancellationToken);
            if (creator == null)
            {
                return Result.Failure<GetCreatorByIdResponse>(Error.ErrorWithValue(
                    CreatorErrors.General_CreatorNotFound,
                    request.Id));
            }

            var result = _mapper.Map<GetCreatorByIdResponse>(creator);
            result.Publications = await GetCreatorPublications(result.Id, cancellationToken);
            result.Artworks = await GetCreatorArtworks(result.Id, cancellationToken);

            await _cache.SetStringAsync(
                CreatorCachingConstants.GetByIdKey + request.Id.ToString(),
                JsonConvert.SerializeObject(result),
                CachingOptionConstants.LongCachingOption,
                cancellationToken);

            return Result.SuccessNullError(result);
        }

        private async Task<List<CreatorTitle>> GetCreatorPublications(Guid creatorId, CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            return await titleRepository.ToListAsync(
                titleRepository.GetQueryableSet()
                    .Where(x => x.TitleAuthors.Any(t => t.AuthorId == creatorId) && !x.IsDeleted && !x.IsHidden)
                    .Select(x => new CreatorTitle(x.Id, x.Name, x.CoverImageUrl)),
                cancellationToken);
        }

        private async Task<List<CreatorTitle>> GetCreatorArtworks(Guid creatorId, CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            return await titleRepository.ToListAsync(
                titleRepository.GetQueryableSet()
                    .Where(x => x.TitleArtists.Any(t => t.ArtistId == creatorId) && !x.IsDeleted && !x.IsHidden)
                    .Select(x => new CreatorTitle(x.Id, x.Name, x.CoverImageUrl)),
                cancellationToken);
        }
    }
}
