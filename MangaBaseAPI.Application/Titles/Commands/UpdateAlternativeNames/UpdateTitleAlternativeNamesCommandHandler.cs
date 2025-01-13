using MangaBaseAPI.Application.Titles.Commands.Create;
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

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAlternativeNames
{
    public class UpdateTitleAlternativeNamesCommandHandler
        : IRequestHandler<UpdateTitleAlternativeNamesCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _distributedCache;

        public UpdateTitleAlternativeNamesCommandHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<Result> Handle(
            UpdateTitleAlternativeNamesCommand request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleAlternativeNamesSpecification(request.Id)));

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            var invalidAlternativeNames = await FindInvalidAlternativeNames(request.AlternativeNames);
            if (invalidAlternativeNames.Any())
            {
                return Result.Failure(Error.Validation(
                    TitleErrors.Update_InvalidAltName.Code,
                    TitleErrors.Update_InvalidAltName.Description,
                    invalidAlternativeNames));
            }

            title.AlternativeNames.Clear();
            foreach (var newName in request.AlternativeNames)
            {
                title.AlternativeNames.Add(new AlternativeName(title.Id, newName.Name, newName.LanguageCodeId));
            }

            titleRepository.UpdateAsync(title);
            var updateResult = await _unitOfWork.SaveChangeAsync();
            if (updateResult == 0)
            {
                return Result.Failure(TitleErrors.Update_UpdateAltNameFailed);
            }

            _ = _distributedCache.RemoveAsync(TitleCachingConstants.GetByIdKey + request.Id.ToString(), cancellationToken);

            return Result.SuccessNullError();
        }

        private async Task<List<TitleAlternativeName>> FindInvalidAlternativeNames(
            List<TitleAlternativeName> newNames)
        {
            if (newNames.Count == 0)
            {
                return newNames;
            }

            var cachedLanguages = await _distributedCache.GetStringAsync(LanguageCodeCachingConstants.GetAllKey);
            var newNameLanguageCodeIds = new HashSet<string>(newNames.Select(n => n.LanguageCodeId));

            if (string.IsNullOrEmpty(cachedLanguages))
            {
                var languageRepository = _unitOfWork.GetRepository<ILanguageCodeRepository>();
                var existingLanguagesIds = await languageRepository.GetQueryableSet()
                    .Select(x => x.Id)
                    .Where(x => newNameLanguageCodeIds.Contains(x))
                    .ToListAsync();

                return newNames.Where(x => !existingLanguagesIds.Contains(x.LanguageCodeId)).ToList();
            }

            var cachedExistingLanguagesIds = JsonConvert
                .DeserializeObject<List<LanguageCodeResponse>>(cachedLanguages)!
                .Select(x => x.Id)
                .Where(newNameLanguageCodeIds.Contains)
                .ToList();

            return newNames.Where(x => !cachedExistingLanguagesIds!.Contains(x.LanguageCodeId)).ToList();
        }
    }
}
