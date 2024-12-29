using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Infrastructure
{
    public class CloudStorageErrors
    {
        #region Google
        public static readonly Error Google_RemoveFileFailed = Error.Failure("CloudStorage.Google.RemoveFileFailed", "Unexpected error(s) occurred when trying to remove file from cloud storage");
        public static readonly Error Google_UploadFileFailed = Error.Failure("CloudStorage.Google.UploadFileFailed", "Unexpected error(s) occurred when trying to upload file to cloud storage");
        #endregion
    }
}
