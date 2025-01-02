using MangaBaseAPI.Contracts.LanguageCodes.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Titles.Commands.Create
{
    public class CreateTitleCommandHandler
        : IRequestHandler<CreateTitleCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _distributedCache;

        public CreateTitleCommandHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<Result> Handle(
            CreateTitleCommand request,
            CancellationToken cancellationToken)
        {
            // Check for title with same exact name
            var titleRepo = _unitOfWork.GetRepository<ITitleRepository>();
            if (await titleRepo.IsTitleNameTaken(request.Name))
            {
                return Result.Failure(TitleErrors.Create_ExistedTitleName);
            }

            var invalidAltNames = await FindInvalidAlternativeNames(request.AlternativeNames);
            if (invalidAltNames.Any())
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidAltNameLanguage.Code,
                    TitleErrors.Create_InvalidAltNameLanguage.Description,
                    invalidAltNames));
            }

            var invalidGenres = await FindInvalidGenres(request.Genres);
            if (invalidGenres.Any())
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidGenre.Code,
                    TitleErrors.Create_InvalidGenre.Description,
                    invalidGenres));
            }

            var existingTitleCreatorsIds = _unitOfWork.GetRepository<ICreatorRepository>()
                .GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => (request.Authors != null && request.Authors.Contains(x)) || (request.Artists != null && request.Artists.Contains(x)))
                .ToHashSet();

            var invalidAuthors = FindInvalidCreators(request.Authors, existingTitleCreatorsIds);
            if (invalidAuthors.Any())
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidAuthor.Code,
                    TitleErrors.Create_InvalidAuthor.Description,
                    invalidAuthors));
            }

            var invalidArtists = FindInvalidCreators(request.Artists, existingTitleCreatorsIds);
            if (invalidArtists.Any())
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidArtist.Code,
                    TitleErrors.Create_InvalidArtist.Description,
                    invalidArtists));
            }

            // Create new title entity
            var newTitleId = Guid.NewGuid();
            var newTitle = new Title(newTitleId,
                request.Name,
                request.Description,
                request.TitleType,
                request.TitleStatus,
                request.PublishedDate,
                request.UploaderId);
            newTitle.TitleGenres = GenerateTitleGenres(request.Genres, newTitleId);
            newTitle.AlternativeNames = GenerateAlternativeNames(request.AlternativeNames, newTitleId);
            newTitle.TitleAuthors = GenerateTitleAuthors(request.Authors, newTitleId);
            newTitle.TitleArtists = GenerateTitleArtists(request.Artists, newTitleId);

            // Save title to database
            await titleRepo.AddAsync(newTitle, cancellationToken);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result == 0)
            {
                return Result.Failure(TitleErrors.Create_CreateTitleFailed);
            }

            return Result.SuccessNullError();
        }

        private List<TitleGenre> GenerateTitleGenres(List<int>? newGenresId, Guid titleId)
        {
            var result = new List<TitleGenre>();
            if (newGenresId == null || newGenresId.Count == 0)
            {
                return result;
            }

            foreach (var genreId in newGenresId)
            {
                result.Add(new TitleGenre(titleId, genreId));
            }
            return result;
        }

        private List<AlternativeName> GenerateAlternativeNames(List<TitleAlternativeName>? altNames, Guid titleId)
        {
            var result = new List<AlternativeName>();
            if (altNames == null || altNames.Count == 0)
            {
                return result;
            }

            foreach (var altName in altNames)
            {
                result.Add(new AlternativeName(
                    titleId,
                    altName.Name,
                    altName.LanguageCodeId));
            }

            return result;
        }

        private List<TitleAuthor> GenerateTitleAuthors(List<Guid>? authorIds, Guid titleId)
        {
            var result = new List<TitleAuthor>();
            if (authorIds == null || authorIds.Count == 0)
            {
                return result;
            }

            foreach (var authorId in authorIds)
            {
                result.Add(new TitleAuthor(titleId, authorId));
            }

            return result;
        }

        private List<TitleArtist> GenerateTitleArtists(List<Guid>? artistIds, Guid titleId)
        {
            var result = new List<TitleArtist>();
            if (artistIds == null || artistIds.Count == 0)
            {
                return result;
            }

            foreach (var artistId in artistIds)
            {
                result.Add(new TitleArtist(titleId, artistId));
            }

            return result;
        }

        private async Task<List<TitleAlternativeName>> FindInvalidAlternativeNames(List<TitleAlternativeName>? newAltNames)
        {
            if (newAltNames == null || newAltNames.Count == 0)
            {
                return new List<TitleAlternativeName>();
            }

            var cachedLanguages = await _distributedCache.GetStringAsync(LanguageCodeCachingConstants.GetAllKey);
            var newNameLanguageCodeIds = new HashSet<string>(newAltNames.Select(n => n.LanguageCodeId));

            if (string.IsNullOrEmpty(cachedLanguages))
            {
                var languageRepository = _unitOfWork.GetRepository<ILanguageCodeRepository>();
                var existingLanguagesIds = await languageRepository.GetQueryableSet()
                    .Select(x => x.Id)
                    .Where(x => newNameLanguageCodeIds.Contains(x))
                    .ToListAsync();

                return newAltNames.Where(x => !existingLanguagesIds.Contains(x.LanguageCodeId)).ToList();
            }

            var cachedExistingLanguagesIds = JsonConvert
                .DeserializeObject<List<LanguageCodeResponse>>(cachedLanguages)!
                .Select(x => x.Id)
                .Where(newNameLanguageCodeIds.Contains)
                .ToList();

            return newAltNames.Where(x => !cachedExistingLanguagesIds!.Contains(x.LanguageCodeId)).ToList();
        }

        private async Task<List<int>> FindInvalidGenres(List<int>? newGenres)
        {
            if (newGenres == null || newGenres.Count == 0)
            {
                return new List<int>();
            }

            var existingGenreIds = await _unitOfWork.GetRepository<IGenreRepository>()
                .GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => newGenres.Contains(x))
                .ToListAsync();

            return newGenres.Except(existingGenreIds).ToList();
        }

        private List<Guid> FindInvalidCreators(List<Guid>? titleCreators, HashSet<Guid> creatorIds)
        {
            if (titleCreators == null || titleCreators.Count == 0)
            {
                return new List<Guid>();
            }

            var existingCreatorIds = creatorIds.Where(titleCreators.Contains).ToList();

            return titleCreators.Except(existingCreatorIds).ToList();
        }
    }
}
