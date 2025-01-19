using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateArtists
{
    public class UpdateTitleArtistsCommandHandler
        : IRequestHandler<UpdateTitleArtistsCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _cache;

        public UpdateTitleArtistsCommandHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result> Handle(
            UpdateTitleArtistsCommand request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleArtistsSpecification(request.Id)),
                cancellationToken);

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            var invalidArtists = await FindInvalidNewArtists(request.Artists, cancellationToken);
            if (invalidArtists.Any())
            {
                return Result.Failure(Error.Validation(
                    TitleErrors.Update_InvalidArtist.Code,
                    TitleErrors.Update_InvalidArtist.Description,
                    invalidArtists));
            }

            title.TitleArtists.Clear();
            foreach (var newArtistId in request.Artists)
            {
                title.TitleArtists.Add(new TitleArtist(title.Id, newArtistId));
            }

            titleRepository.Update(title);
            var updateResult = await _unitOfWork.SaveChangeAsync();
            if (updateResult == 0)
            {
                return Result.Failure(TitleErrors.Update_UpdateArtistFailed);
            }

            _ = _cache.RemoveAsync(ChapterCachingConstants.GetByIdKey + request.Id, cancellationToken);

            return Result.SuccessNullError();
        }

        private async Task<List<Guid>> FindInvalidNewArtists(List<Guid> newArtists, CancellationToken cancellationToken)
        {
            if (newArtists.Count == 0)
            {
                return newArtists;
            }

            var artistRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            var existingArtist = await artistRepository.GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => newArtists.Contains(x))
                .ToListAsync(cancellationToken);

            return newArtists.Except(existingArtist).ToList();
        }
    }
}
