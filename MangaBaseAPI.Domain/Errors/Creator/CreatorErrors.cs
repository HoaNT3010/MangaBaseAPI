using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Creator
{
    public class CreatorErrors
    {
        #region Create
        public static readonly Error Create_ExistedCreatorName = Error.Conflict("Creator.Create.ExistedCreatorName", "A creator with the same name has existed. Only new creators can be added");
        public static readonly Error Create_CreateCreatorFailed = Error.Failure("Creator.Create.CreateCreatorFailed", "Unexpected error(s) occurred when trying to create new creator");
        #endregion

        #region General
        public static readonly Error General_CreatorNotFound = Error.NotFound("Creator.General.CreatorNotFound", "No creator found with the given ID");
        #endregion
    }
}
