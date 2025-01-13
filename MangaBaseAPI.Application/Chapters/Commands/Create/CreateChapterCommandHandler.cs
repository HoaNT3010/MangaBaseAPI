using MangaBaseAPI.Application.Common.Utilities.Storage;
using MangaBaseAPI.Contracts.Common.Response;
using MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Location;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Chapter;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Chapters.Commands.Create
{
    public class CreateChapterCommandHandler
        : IRequestHandler<CreateChapterCommand, Result<PostRequestResponse>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IGoogleCloudStorageService _cloudStorage;
        readonly ILogger<CreateChapterCommandHandler> _logger;

        public CreateChapterCommandHandler(
            IUnitOfWork unitOfWork,
            IGoogleCloudStorageService cloudStorage,
            ILogger<CreateChapterCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cloudStorage = cloudStorage;
            _logger = logger;
        }

        public async Task<Result<PostRequestResponse>> Handle(
            CreateChapterCommand request,
            CancellationToken cancellationToken)
        {
            var isTitleExist = await _unitOfWork.GetRepository<ITitleRepository>()
                .GetQueryableSet()
                .AnyAsync(x => x.Id == request.TitleId, cancellationToken);

            if (!isTitleExist)
            {
                return Result.Failure<PostRequestResponse>(Error.ErrorWithValue(TitleErrors.General_TitleNotFound, request.TitleId));
            }

            var chapterRepository = _unitOfWork.GetRepository<IChapterRepository>();

            // Check if chapter with same name or index already exists
            string trimmedChapterName = request.Name.Trim();
            var duplicatedNamesAndIndexes = await chapterRepository.GetQueryableSet()
                .Where(x => (x.Name.ToLower() == trimmedChapterName.ToLower() || x.Index == request.Index) && !x.IsDeleted)
                .Select(x => new { x.Name, x.Index })
                .ToListAsync(cancellationToken);
            if (duplicatedNamesAndIndexes.Count > 0)
            {
                if (duplicatedNamesAndIndexes.Any(x => string.Compare(x.Name, trimmedChapterName, true) == 0))
                {
                    return Result.Failure<PostRequestResponse>(Error.ErrorWithValue(ChapterErrors.Create_ExistedChapterName, trimmedChapterName));
                }
                if (duplicatedNamesAndIndexes.Any(x => x.Index == request.Index))
                {
                    return Result.Failure<PostRequestResponse>(Error.ErrorWithValue(ChapterErrors.Create_ExistedChapterIndex, request.Index));
                }
            }

            Chapter newChapter = new Chapter(Guid.NewGuid(),
                trimmedChapterName,
                request.Index,
                request.Volume,
                request.TitleId,
                request.UploaderId);
            var chapterImageNames = GenerateImageNames(request.ChapterImages, request.TitleId, request.Index);
            newChapter.ChapterImages = GenerateChapterImages(chapterImageNames, newChapter.Id);

            (bool uploadImageResult, int successfulUpload) = await UploadChapterImages(chapterImageNames, request.ChapterImages);
            if (!uploadImageResult || successfulUpload < request.ChapterImages.Count)
            {
                await DeleteChapterImages(chapterImageNames, successfulUpload);
                return Result.Failure<PostRequestResponse>(ChapterErrors.Create_UploadImagesFailed);
            }

            try
            {
                await chapterRepository.AddAsync(newChapter);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create new chapter: {Message}", ex.Message);
                await DeleteChapterImages(chapterImageNames, successfulUpload);
                return Result.Failure<PostRequestResponse>(ChapterErrors.Create_CreateChapterFailed);
            }

            return Result.SuccessNullError(new PostRequestResponse($"{LocationConstants.ApiV1BaseLocation}{LocationConstants.ChapterResource}{newChapter.Id.ToString()}"));
        }

        private List<string> GenerateImageNames(IFormFileCollection images, Guid titleId, float chapterIndex)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < images.Count; i++)
            {
                result.Add(FilePathGenerator.GenerateChapterImagePath(titleId.ToString(),
                    chapterIndex.ToString(),
                    (i + 1).ToString() + Path.GetExtension(images[i].FileName)));
            }

            return result;
        }

        private List<ChapterImage> GenerateChapterImages(List<string> imagePaths, Guid chapterId)
        {
            List<ChapterImage> result = new List<ChapterImage>();
            for (int i = 0; i < imagePaths.Count; i++)
            {
                result.Add(new ChapterImage(chapterId,
                    FilePathGenerator.GenerateFullFileUrl(_cloudStorage.GetBucketName(), imagePaths[i]),
                    i + 1));
            }
            return result;
        }

        private async Task<(bool, int)> UploadChapterImages(List<string> imageNames, IFormFileCollection images)
        {
            int successfulUpload = 0;
            try
            {
                for (int i = 0; i < images.Count; i++)
                {
                    await _cloudStorage.UploadFileAsync(images[i], imageNames[i]);
                    successfulUpload++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to upload chapter image {CurrentImageCount}/{TotalImageCount} to cloud storage: {Message}", successfulUpload + 1, images.Count, ex.Message);
                return (false, successfulUpload);
            }
            return (true, successfulUpload);
        }

        private async Task DeleteChapterImages(List<string> chapterNames, int successfulUpload)
        {
            if (successfulUpload == 0)
            {
                return;
            }
            int currentPosition = 0;
            try
            {
                for (int i = 0; i < successfulUpload; i++)
                {
                    await _cloudStorage.DeleteFileAsync(chapterNames[i]);
                    currentPosition++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete chapter image {CurrentImageCount}/{TotalImageCount} from cloud storage: {Message}", currentPosition + 1, successfulUpload, ex.Message);
            }
        }
    }
}
