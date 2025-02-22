namespace MangaBaseAPI.Contracts.Users.GetById
{
    public class GetUserByIdResponse
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public bool IsEmailConfirmed { get; init; } = false;
        public string PhoneNumber { get; init; } = null!;
        public bool IsPhoneNumberConfirmed { get; init; } = false;
        public DateTimeOffset CreatedDateTime { get; init; }
        public DateTimeOffset? ModifiedDateTime { get; init; }
        public bool IsLockoutEnabled { get; init; } = false;
        public DateTimeOffset? LockoutEnd { get; init; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
