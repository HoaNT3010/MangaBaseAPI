using MangaBaseAPI.Application.Common.Utilities.Storage;
using MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler;
using MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Errors.Chapter;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Chapters.Commands.Delete
{
    public class DeleteChapterCommandHandler
        : IRequestHandler<DeleteChapterCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ILogger<DeleteChapterCommandHandler> _logger;
        readonly IHangfireBackgroundJobService _jobService;
        readonly IDistributedCache _cache;

        public DeleteChapterCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<DeleteChapterCommandHandler> logger,
            IHangfireBackgroundJobService jobService,
            IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jobService = jobService;
            _cache = cache;
        }

        public async Task<Result> Handle(
            DeleteChapterCommand request,
            CancellationToken cancellationToken)
        {
            var chapterRepository = _unitOfWork.GetRepository<IChapterRepository>();
            var chapter = await chapterRepository.FirstOrDefaultAsync(
                chapterRepository.ApplySpecification(new DeleteChapterSpecification(request.Id)));

            if (chapter == null)
            {
                return Result.Failure(Error.ErrorWithValue(
                    ChapterErrors.General_ChapterNotFound,
                    request.Id));
            }
            if (chapter.IsDeleted)
            {
                return Result.Failure(Error.ErrorWithValue(
                    ChapterErrors.General_ChapterDeleted,
                    request.Id));
            }

            chapter.IsDeleted = true;
            try
            {
                chapterRepository.UpdateAsync(chapter);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete chapter with ID {ChapterId}: {Message}", request.Id, ex.Message);
                return Result.Failure(ChapterErrors.Delete_DeleteChapterFailed);
            }

            _ = _cache.RemoveAsync(ChapterCachingConstants.GetByIdKey + request.Id, cancellationToken);
            _ = _cache.RemoveAsync(ChapterCachingConstants.GetTitleChaptersListConstant(chapter.TitleId), cancellationToken);

            AddBackgroundTasks(
                request.Id,
                chapter.ChapterImages.Select(x => FilePathGenerator.ExtractFilePath(x.Url)).ToList());

            return Result.SuccessNullError();
        }

        private void AddBackgroundTasks(
            Guid chapterId,
            List<string> chapterImageFilePath)
        {
            try
            {
                _jobService.Enqueue<IGoogleCloudStorageService>(storage => storage.DeleteMultipleFilesAsync(chapterImageFilePath));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add clean up tasks after delete chapter with ID {ChapterId}: {Message}", chapterId, ex.Message);
            }
        }
    }
}
