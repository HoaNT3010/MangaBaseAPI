namespace MangaBaseAPI.Domain.Constants.Role
{
    public class ApplicationRoles
    {
        public const string Administrator = "Administrator";
        public const string Member = "Member";
    }

    public class RoleClaimType
    {
        // Access level
        public const string AccessLevel = "AccessLevel";

        // CanManageUsers
        public const string CanManageUsers = "CanManageUsers";

        // CanManageContent
        public const string CanManageContent = "CanManageContent";
    }

    public class RoleClaimValue
    {
        // Access level
        public const string AccessLevelAdmin = "Admin";
        public const string AccessLevelMember = "Member";

        // CanManageUsers
        public const string CanManageUsers = "True";
        public const string CannotManageUsers = "False";

        // CanManageContent
        public const string CanManageContent = "True";
        public const string CannotManageContent = "False";
    }
}
