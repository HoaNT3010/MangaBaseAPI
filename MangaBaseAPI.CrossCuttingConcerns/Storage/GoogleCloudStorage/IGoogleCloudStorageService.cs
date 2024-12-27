using Microsoft.AspNetCore.Http;

namespace MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage
{
    public interface IGoogleCloudStorageService
    {
        Task UploadFileAsync(string filePath, string destinationFileName);
        Task UploadMultipleFilesAsync(Dictionary<string, string> files);
        Task DeleteFileAsync(string fileName);
        Task DeleteMultipleFilesAsync(List<string> fileNames);

        Task UploadFileAsync(IFormFile file);
        Task UploadMultipleFilesAsync(IEnumerable<IFormFile> files);
        Task UploadFileAsync(IFormFile file, string destinationFileName);
        string GetBucketName();
    }
}
