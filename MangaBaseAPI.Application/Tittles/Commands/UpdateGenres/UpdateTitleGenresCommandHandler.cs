using MangaBaseAPI.Contracts.Genres.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateGenres
{
    public class UpdateTitleGenresCommandHandler
        : IRequestHandler<UpdateTitleGenresCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _distributedCache;

        public UpdateTitleGenresCommandHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<Result> Handle(
            UpdateTitleGenresCommand request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleGenresSpecification(request.Id)));

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            var invalidGenres = await FindInvalidNewGenres(request.Genres);
            if (invalidGenres.Any())
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Update_InvalidGenre.Code,
                    TitleErrors.Update_InvalidGenre.Description + string.Join(" ", invalidGenres)));
            }

            title.TitleGenres.Clear();
            foreach (var newGenreId in request.Genres)
            {
                title.TitleGenres.Add(new TitleGenre(title.Id, newGenreId));
            }

            titleRepository.UpdateAsync(title);
            var updateResult = await _unitOfWork.SaveChangeAsync();

            if (updateResult == 0)
            {
                return Result.Failure(TitleErrors.Update_UpdateGenreFailed);
            }

            return Result.SuccessNullError();
        }

        private async Task<List<int>> FindInvalidNewGenres(List<int> newGenres)
        {
            if (newGenres.Count == 0)
            {
                return newGenres;
            }

            var cachedGenresData = await _distributedCache.GetStringAsync(GenreCachingConstants.GetAllKey);
            if (!string.IsNullOrEmpty(cachedGenresData))
            {
                var cachedExistingGenres = JsonConvert.DeserializeObject<List<GenreResponse>>(cachedGenresData)!
                    .Select(x => x.Id)
                    .Where(x => newGenres.Contains(x))
                    .ToList();

                return newGenres.Except(cachedExistingGenres).ToList();
            }

            var genresRepository = _unitOfWork.GetRepository<IGenreRepository>();
            var existingGenres = await genresRepository.GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => newGenres.Contains(x))
                .ToListAsync();

            return newGenres.Except(existingGenres).ToList();
        }
    }
}
