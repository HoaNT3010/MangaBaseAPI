using MangaBaseAPI.Application.Common.Utilities.Storage;
using MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Errors.Infrastructure;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateCoverImage
{
    public class UpdateTitleCoverImageCommandHandler
        : IRequestHandler<UpdateTitleCoverImageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGoogleCloudStorageService _storageService;
        private readonly ILogger<UpdateTitleCoverImageCommandHandler> _logger;
        readonly IDistributedCache _cache;

        public UpdateTitleCoverImageCommandHandler(
            IUnitOfWork unitOfWork,
            IGoogleCloudStorageService storageService,
            ILogger<UpdateTitleCoverImageCommandHandler> logger,
            IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result> Handle(
            UpdateTitleCoverImageCommand request,
            CancellationToken cancellationToken)
        {
            // Consider adding additional validation logic
            // Retrieve title data
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleCoverImageSpecification(request.Id)),
                cancellationToken);

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            // Change file path
            // Every title will have the same cover image name (cover) but different file extension and path
            var newCoverImagePath = FilePathGenerator.GenerateCoverImagePath(title.Id.ToString(), "cover" + Path.GetExtension(request.File.FileName));
            var oldCoverImagePath = FilePathGenerator.GenerateCoverImagePath(title.Id.ToString(), "cover" + Path.GetExtension(title.CoverImageUrl));

            try
            {
                // Remove old cover image from cloud storage (if any)
                // Deleting file using file path, not cover image url save in the database
                if (!string.IsNullOrEmpty(title.CoverImageUrl))
                {
                    await _storageService.DeleteFileAsync(oldCoverImagePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete old cover image from cloud storage: {Message}", ex.Message);
                return Result.Failure(CloudStorageErrors.Google_RemoveFileFailed);
            }

            try
            {
                // Upload cover image to cloud storage with cover image path
                await _storageService.UploadFileAsync(request.File, newCoverImagePath);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to upload new cover image to cloud storage: {Message}", ex.Message);
                return Result.Failure(CloudStorageErrors.Google_UploadFileFailed);
            }

            try
            {
                // Update title cover image url
                // Combine cover image path with Google Cloud Storage domain and bucket name to create full url
                string fullFilePath = FilePathGenerator.GenerateFullFileUrl(_storageService.GetBucketName(), newCoverImagePath);
                title.CoverImageUrl = fullFilePath;
                title.SetModifyDateTime();
                titleRepository.Update(title);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save title's new cover image url: {Message}", ex.Message);
                return Result.Failure(TitleErrors.Update_UpdateTitleCoverFailed);
            }

            _ = _cache.RemoveAsync(ChapterCachingConstants.GetByIdKey + request.Id, cancellationToken);

            return Result.SuccessNullError();
        }
    }
}
