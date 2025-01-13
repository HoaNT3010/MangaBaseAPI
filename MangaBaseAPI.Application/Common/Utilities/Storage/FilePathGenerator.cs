namespace MangaBaseAPI.Application.Common.Utilities.Storage
{
    public static class FilePathGenerator
    {
        const string TitleFolderName = "titles";
        const string GoogleCloudStorageUrl = "https://storage.googleapis.com";
        const string ChapterFolderName = "chapters";
        const string BaseFileUrl = "https://storage.googleapis.com/mangabase/";

        public static string GenerateCoverImagePath(string titleId, string fileName)
        {
            return $"{TitleFolderName}/{titleId}/{fileName}";
        }

        public static string GenerateChapterImagePath(string titleId, string chapterIndex, string fileName)
        {
            return $"{TitleFolderName}/{titleId}/{ChapterFolderName}/{chapterIndex}/{fileName}";
        }

        /// <summary>
        /// Generate the folder path for storing images of the chapter. The path ends with forward slash '/'.
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="chapterIndex"></param>
        /// <returns></returns>
        public static string GenerateChapterImageFolderPath(string titleId, string chapterIndex)
        {
            return $"{TitleFolderName}/{titleId}/{ChapterFolderName}/{chapterIndex}/";
        }

        public static string GenerateFullFileUrl(string bucketName, string filePath)
        {
            return $"{GoogleCloudStorageUrl}/{bucketName}/{filePath}";
        }

        public static string GenerateFullFileUrl(string filePath)
        {
            return $"{BaseFileUrl}{filePath}";
        }

        public static string ExtractFilePath(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new InvalidOperationException("File URL cannot be null or empty");
            }

            return fileUrl.Substring(BaseFileUrl.Length);
        }
    }
}
