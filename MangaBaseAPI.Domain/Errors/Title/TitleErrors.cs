using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Title
{
    public class TitleErrors
    {
        #region Create
        public static readonly Error ExistedTitleName = Error.Conflict("Title.Create.ExistedTitleName", "A title with the same name has existed. Only new title can be created");
        public static readonly Error CreateTitleFailed = Error.Failure("Title.Create.CreateTitleFailed", "Unexpected error(s) occurred when trying to create new title");
        public static readonly Error InvalidAltNameLanguage = Error.Validation("Title.Create.InvalidAltNameLanguage", "Title contains one or many alternative name(s) with invalid language code: ");
        public static readonly Error InvalidGenre = Error.Validation("Title.Create.InvalidGenres", "Title contains one or many invalid genre(s): ");
        public static readonly Error InvalidAuthor = Error.Validation("Title.Create.InvalidAuthor", "Title contains one or many invalid author(s): ");
        public static readonly Error InvalidArtist = Error.Validation("Title.Create.InvalidArtist", "Title contains one or many invalid artist(s): ");
        #endregion
    }
}
