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
                return Result.Failure(TitleErrors.TitleNotFound);
            }

            (bool validateResult, int genreIndex) = await IsNewGenresValid(request.Genres, _unitOfWork, _distributedCache);
            if (!validateResult)
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Update_InvalidGenre.Code,
                    TitleErrors.Update_InvalidGenre.Description + $"Genre with ID '{request.Genres[genreIndex]}'"));
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

        private async Task<(bool, int)> IsNewGenresValid(List<int> newGenres,
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache)
        {
            if (newGenres.Count == 0)
            {
                return (true, -1);
            }

            var cachedGenresData = await _distributedCache.GetStringAsync(GenreCachingConstants.GetAllKey);
            if (!string.IsNullOrEmpty(cachedGenresData))
            {
                var cachedGenres = JsonConvert.DeserializeObject<List<GenreResponse>>(cachedGenresData);
                for (int i = 0; i < newGenres.Count; i++)
                {
                    if (!cachedGenres!.Any(x => x.Id == newGenres[i]))
                    {
                        return (false, i);
                    }
                }
            }

            var genres = await _unitOfWork.GetRepository<IGenreRepository>()
                .GetQueryableSet()
                .ToListAsync();
            for (int i = 0; i < newGenres.Count; i++)
            {
                if (!genres.Any(x => x.Id == newGenres[i]))
                {
                    return (false, i);
                }
            }

            return (true, -1);
        }
    }
}
