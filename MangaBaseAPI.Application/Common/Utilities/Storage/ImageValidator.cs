using Microsoft.AspNetCore.Http;

namespace MangaBaseAPI.Application.Common.Utilities.Storage
{
    public static class ImageValidator
    {
        static readonly string[] AllowedImageExtensions = { ".png", ".jpg", ".jpeg", ".webp" };
        static readonly string[] AllowedMimeTypes = { "image/png", "image/jpeg", "image/webp" };

        // 5MB
        const long MaxFileSize = 5 * 1024 * 1024;

        public static bool IsImageExtensionValid(string fileName)
        {
            return AllowedImageExtensions.Contains(Path.GetExtension(fileName).ToLower());
        }

        public static bool IsFileSizeValid(IFormFile file)
        {
            return file.Length <= MaxFileSize;
        }

        public static bool IsFileMimeTypeValid(IFormFile file)
        {
            return AllowedMimeTypes.Contains(file.ContentType.ToLower());
        }
    }
}
