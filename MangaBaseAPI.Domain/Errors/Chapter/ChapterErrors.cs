using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Chapter
{
    public class ChapterErrors
    {
        #region Create
        public static readonly Error Create_ExistedChapterName = Error.Conflict("Chapter.Create.ExistedChapterName", "A chapter with the same name has existed. Only new chapter with unique name can be created");
        public static readonly Error Create_ExistedChapterIndex = Error.Conflict("Chapter.Create.ExistedChapterIndex", "A chapter with the same index has existed. Only new chapter with unique index can be created");
        public static readonly Error Create_CreateChapterFailed = Error.Failure("Chapter.Create.CreateChapterFailed", "Unexpected error(s) occurred when trying to create new chapter");
        public static readonly Error Create_UploadImagesFailed = Error.Failure("Chapter.Create.UploadImagesFailed", "Unexpected error(s) occurred when trying to upload images of new chapter to storage");
        #endregion

        #region Update
        public static readonly Error Update_UpdateChapterFailed = Error.Failure("Chapter.Update.UpdateChapterFailed", "Unexpected error(s) occurred when trying to update chapter");
        #endregion

        #region General
        public static readonly Error General_ChapterNotFound = Error.NotFound("Chapter.General.ChapterNotFound", "No chapter found with the given ID");
        public static readonly Error General_ChapterDeleted = Error.Forbidden("Chapter.General.ChapterDeleted", "Chapter with the given ID has been deleted and cannot be access");
        #endregion

        #region Delete
        public static readonly Error Delete_DeleteChapterFailed = Error.Failure("Chapter.Delete.DeleteChapterFailed", "Unexpected error(s) occurred when trying to delete chapter");
        #endregion
    }
}
