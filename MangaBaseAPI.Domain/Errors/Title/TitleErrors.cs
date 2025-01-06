using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Title
{
    public class TitleErrors
    {
        #region Create
        public static readonly Error Create_ExistedTitleName = Error.Conflict("Title.Create.ExistedTitleName", "A title with the same name has existed. Only new title with unique name can be created");
        public static readonly Error Create_CreateTitleFailed = Error.Failure("Title.Create.CreateTitleFailed", "Unexpected error(s) occurred when trying to create new title");
        public static readonly Error Create_InvalidAltNameLanguage = Error.Validation("Title.Create.InvalidAltNameLanguage", "Title contains one or many name(s) with invalid language code ID");
        public static readonly Error Create_InvalidGenre = Error.Validation("Title.Create.InvalidGenres", "Title contains one or many genre(s) with invalid ID");
        public static readonly Error Create_InvalidAuthor = Error.Validation("Title.Create.InvalidAuthor", "Title contains one or many author(s) invalid creator ID");
        public static readonly Error Create_InvalidArtist = Error.Validation("Title.Create.InvalidArtist", "Title contains one or many artist(s) with invalid creator ID");
        #endregion

        #region Update
        public static readonly Error Update_UpdateTitleCoverFailed = Error.Failure("Title.Update.UpdateTitleCoverFailed", "Unexpected error(s) occurred when trying to update title's cover image");
        public static readonly Error Update_InvalidGenre = Error.Validation("Title.Update.InvalidGenres", "Title's new genres contains one or many genre(s) with invalid ID");
        public static readonly Error Update_UpdateGenreFailed = Error.Failure("Title.Update.UpdateGenreFailed", "Unexpected error(s) occurred when trying to update title's genres");
        public static readonly Error Update_InvalidAuthor = Error.Validation("Title.Update.InvalidAuthor", "Title's new authors contains one or many author(s) with invalid creator ID");
        public static readonly Error Update_UpdateAuthorFailed = Error.Failure("Title.Update.UpdateAuthorFailed", "Unexpected error(s) occurred when trying to update title's authors");
        public static readonly Error Update_InvalidArtist = Error.Validation("Title.Update.InvalidArtist", "Title's new artists contains one or many artist(s) with invalid creator ID");
        public static readonly Error Update_UpdateArtistFailed = Error.Failure("Title.Update.UpdateArtistFailed", "Unexpected error(s) occurred when trying to update title's artists");
        public static readonly Error Update_InvalidAltName = Error.Validation("Title.Update.InvalidAltName", "Title's new alternative names contains one or many name(s) with invalid language code ID");
        public static readonly Error Update_UpdateAltNameFailed = Error.Failure("Title.Update.UpdateAltNameFailed", "Unexpected error(s) occurred when trying to update title's alternative names");
        #endregion

        #region General
        public static readonly Error General_TitleNotFound = Error.NotFound("Title.General.TitleNotFound", "No title found with the given ID");
        public static readonly Error General_TitleHidden = Error.Forbidden("Title.General.TitleHidden", "Title with the given ID has been hidden and cannot be access");
        public static readonly Error General_TitleDeleted = Error.Forbidden("Title.General.TitleDeleted", "Title with the given ID has been deleted and cannot be access");
        #endregion
    }
}
