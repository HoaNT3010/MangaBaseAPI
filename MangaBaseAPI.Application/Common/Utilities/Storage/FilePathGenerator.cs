namespace MangaBaseAPI.Application.Common.Utilities.Storage
{
    public static class FilePathGenerator
    {
        const string TitleFolderName = "titles";

        public static string GenerateCoverImagePath(string titleId, string fileName)
        {
            return $"{TitleFolderName}/{titleId}/{fileName}";
        }

        public static string GenerateChapterImagePath(string titleId, string chapterIndex, string fileName)
        {
            return $"{TitleFolderName}/{titleId}/{chapterIndex}/{fileName}";
        }

        /// <summary>
        /// Generate the folder path for storing images of the chapter. The path ends with forward slash '/'.
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="chapterIndex"></param>
        /// <returns></returns>
        public static string GenerateChapterImageFolderPath(string titleId, string chapterIndex)
        {
            return $"{TitleFolderName}/{titleId}/{chapterIndex}/";
        }

        public static string GenerateFileUrl(string bucketUrl, string filePath)
        {
            return $"{bucketUrl}/{filePath}";
        }
    }
}
