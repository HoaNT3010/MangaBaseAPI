namespace MangaBaseAPI.Domain.Constants.Caching
{
    public class ChapterCachingConstants
    {
        public const string GetByIdKey = "Chapter_";

        public static string GetTitleChaptersListConstant(Guid titleId)
        {
            return $"Title_{titleId.ToString()}_ChaptersList";
        }
    }
}
