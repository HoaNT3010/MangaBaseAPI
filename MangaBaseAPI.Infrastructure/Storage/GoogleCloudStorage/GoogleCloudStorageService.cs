using Google.Cloud.Storage.V1;
using MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MangaBaseAPI.Infrastructure.Storage.GoogleCloudStorage
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        readonly StorageClient _storageClient;
        readonly GoogleCloudStorageOptions _storageOptions;
        readonly ILogger<GoogleCloudStorageService> _logger;


        public GoogleCloudStorageService(
            IOptions<GoogleCloudStorageOptions> storageOptions,
            StorageClient storageClient,
            ILogger<GoogleCloudStorageService> logger)
        {
            _storageOptions = storageOptions.Value;
            _storageClient = storageClient;
            _logger = logger;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _storageClient.DeleteObjectAsync(_storageOptions.BucketName, fileName);

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully deleted file {@FileName} from cloud storage",
                typeof(GoogleCloudStorageService).Name,
                nameof(DeleteFileAsync),
                fileName);
        }

        public async Task DeleteMultipleFilesAsync(List<string> fileNames)
        {
            if (fileNames.Count == 0)
            {
                _logger.LogInformation("{@ServiceName} - {@MethodName}: Empty files list, cancelled files deleting operation",
                typeof(GoogleCloudStorageService).Name,
                nameof(DeleteFileAsync));

                return;
            }

            foreach (var fileName in fileNames)
            {
                await DeleteFileAsync(fileName);
            }

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully deleted {@FileCount} file(s) from cloud storage",
                typeof(GoogleCloudStorageService).Name,
                nameof(DeleteFileAsync),
                fileNames.Count);
        }

        public async Task UploadFileAsync(string filePath, string destinationFileName)
        {
            using var fileStream = File.OpenRead(filePath);

            await _storageClient.UploadObjectAsync(_storageOptions.BucketName,
                destinationFileName,
                null,
                fileStream);

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully uploaded file {@FileName} to cloud storage",
                typeof(GoogleCloudStorageService).Name,
                nameof(UploadFileAsync),
                destinationFileName);
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            string destinationFileName = file.FileName;

            using var stream = file.OpenReadStream();
            await _storageClient.UploadObjectAsync(_storageOptions.BucketName,
                destinationFileName,
                null,
                stream);

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully uploaded file {@FileName} to cloud storage",
                typeof(GoogleCloudStorageService).Name,
                nameof(UploadFileAsync),
                destinationFileName);
        }

        public async Task UploadMultipleFilesAsync(Dictionary<string, string> files)
        {
            if (files.Count == 0)
            {
                return;
            }

            foreach (var file in files)
            {
                await UploadFileAsync(file.Key, file.Value);
            }

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully uploaded {@FileCount} file(s) to cloud storage",
               typeof(GoogleCloudStorageService).Name,
               nameof(UploadMultipleFilesAsync),
               files.Count);
        }

        public async Task UploadMultipleFilesAsync(IEnumerable<IFormFile> files)
        {
            if (files == null || files.Count() == 0)
            {
                throw new ArgumentException("No files provided for upload.");
            }

            foreach (var file in files)
            {
                await UploadFileAsync(file);
            }

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully uploaded {@FileCount} file(s) to cloud storage",
                typeof(GoogleCloudStorageService).Name,
                nameof(DeleteFileAsync),
                files.Count());
        }

        public async Task UploadFileAsync(IFormFile file, string destinationFileName)
        {
            if (file is null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            using var stream = file.OpenReadStream();
            await _storageClient.UploadObjectAsync(_storageOptions.BucketName,
                destinationFileName,
                file.ContentType,
                stream);

            _logger.LogInformation("{@ServiceName} - {@MethodName}: Successfully uploaded file {@FileName} to cloud storage",
                typeof(GoogleCloudStorageService).Name,
                nameof(UploadFileAsync),
                destinationFileName);
        }

        public string GetBucketName()
        {
            return _storageOptions.BucketName;
        }
    }
}
