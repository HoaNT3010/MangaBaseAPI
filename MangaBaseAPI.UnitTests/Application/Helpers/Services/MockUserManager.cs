using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace MangaBaseAPI.UnitTests.Application.Helpers.Services
{
    public static class MockUserManager
    {
        public static UserManager<TUser> CreateMock<TUser>() where TUser : class
        {
            var store = Substitute.For<IUserStore<TUser>>();
            return Substitute.For<UserManager<TUser>>(
                store,
                null, null, null, null, null, null, null, null);
        }
    }
}
