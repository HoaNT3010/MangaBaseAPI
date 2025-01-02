using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Commands.Create
{
    public class CreateTitleCommandHandler
        : IRequestHandler<CreateTitleCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;

        public CreateTitleCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            // Validate title's properties (alt names, genres, authors, artists)
            (bool altNameValidationResult, int altNameIndex) = ValidateAlternativeNames(request, _unitOfWork);
            if (!altNameValidationResult)
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidAltNameLanguage.Code,
                    TitleErrors.Create_InvalidAltNameLanguage.Description + $"Position '{altNameIndex + 1}' - Name '{request.AlternativeNames![altNameIndex].Name}' - Language code '{request.AlternativeNames![altNameIndex].LanguageCodeId}'"));
            }

            (bool genreValidationResult, int genreIndex) = ValidateGenres(request.Genres, _unitOfWork);
            if (!genreValidationResult)
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidGenre.Code,
                    TitleErrors.Create_InvalidGenre.Description + $"Genre at position '{genreIndex + 1}'"));
            }

            var creatorIds = _unitOfWork.GetRepository<ICreatorRepository>()
                .GetQueryableSet()
                .Select(x => x.Id)
                .ToHashSet();

            (bool authorValidationResult, int authorIndex) = ValidateTitleCreators(request.Authors, creatorIds);
            if (!authorValidationResult)
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidAuthor.Code,
                    TitleErrors.Create_InvalidAuthor.Description + $"Author at position '{authorIndex + 1}' - Id '{request.Authors![authorIndex]}'"));
            }

            (bool artistValidationResult, int artistIndex) = ValidateTitleCreators(request.Artists, creatorIds);
            if (!artistValidationResult)
            {
                return Result.Failure(
                    Error.Validation(TitleErrors.Create_InvalidArtist.Code,
                    TitleErrors.Create_InvalidArtist.Description + $"Artist at position '{authorIndex + 1}' - Id '{request.Artists![authorIndex]}'"));
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

            // Generate title's alternative names, genres, authors, and artists
            PopulateTitleGenres(request, newTitle);
            PopulateAlternativeNames(request, newTitle);
            PopulateTitleAuthors(request, newTitle);
            PopulateTitleArtists(request, newTitle);

            // Save title to database
            await titleRepo.AddAsync(newTitle, cancellationToken);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result == 0)
            {
                return Result.Failure(TitleErrors.Create_CreateTitleFailed);
            }

            return Result.SuccessNullError();
        }

        private void PopulateTitleGenres(CreateTitleCommand command, Title title)
        {
            if (command.Genres == null || command.Genres.Count == 0)
            {
                return;
            }

            foreach (var genre in command.Genres)
            {
                TitleGenre newTitleGenre = new TitleGenre(
                    title.Id,
                    genre);

                title.TitleGenres.Add(newTitleGenre);
            }
        }

        private void PopulateAlternativeNames(CreateTitleCommand command, Title title)
        {
            if (command.AlternativeNames == null || command.AlternativeNames.Count == 0)
            {
                return;
            }

            foreach (var altName in command.AlternativeNames)
            {
                AlternativeName newAltName = new AlternativeName(
                    title.Id,
                    altName.Name,
                    altName.LanguageCodeId);

                title.AlternativeNames.Add(newAltName);
            }
        }

        private void PopulateTitleAuthors(CreateTitleCommand command, Title title)
        {
            if (command.Authors == null || command.Authors.Count == 0)
            {
                return;
            }

            foreach (var authorId in command.Authors)
            {
                TitleAuthor newAuthor = new TitleAuthor(
                    title.Id,
                    authorId);

                title.TitleAuthors.Add(newAuthor);
            }
        }

        private void PopulateTitleArtists(CreateTitleCommand command, Title title)
        {
            if (command.Artists == null || command.Artists.Count == 0)
            {
                return;
            }

            foreach (var artistId in command.Artists)
            {
                TitleArtist newArtist = new TitleArtist(
                    title.Id,
                    artistId);

                title.TitleArtists.Add(newArtist);
            }
        }

        private (bool, int) ValidateAlternativeNames(CreateTitleCommand command, IUnitOfWork unitOfWork)
        {
            if (command.AlternativeNames == null || command.AlternativeNames.Count == 0)
            {
                return (true, -1);
            }

            var languageCodeIds = _unitOfWork.GetRepository<ILanguageCodeRepository>()
                .GetQueryableSet()
                .Select(x => x.Id)
                .ToHashSet();

            for (int i = 0; i < command.AlternativeNames.Count; i++)
            {
                if (!languageCodeIds.Contains(command.AlternativeNames[i].LanguageCodeId))
                {
                    return (false, i);
                }
            }

            return (true, -1);
        }

        private (bool, int) ValidateGenres(List<int>? genres, IUnitOfWork unitOfWork)
        {
            if (genres == null || genres.Count == 0)
            {
                return (true, -1);
            }

            var genreIds = unitOfWork.GetRepository<IGenreRepository>()
                .GetQueryableSet()
                .Select(x => x.Id)
                .ToHashSet();

            for (int i = 0; i < genres.Count; i++)
            {
                if (!genreIds.Contains(genres[i]))
                {
                    return (false, i);
                }
            }

            return (true, -1);
        }

        private (bool, int) ValidateTitleCreators(List<Guid>? titleCreators, HashSet<Guid> creatorIds)
        {
            if (titleCreators == null || titleCreators.Count == 0)
            {
                return (true, -1);
            }

            for (int i = 0; i < titleCreators.Count; i++)
            {
                if (!creatorIds.Contains(titleCreators[i]))
                {
                    return (false, i);
                }
            }

            return (true, -1);
        }
    }
}
