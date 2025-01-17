using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Constants.User;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Authentication.Common.Specifications
{
    internal class GetUserEmailVerificationTokenSpecification : Specification<UserToken>
    {
        public GetUserEmailVerificationTokenSpecification(Guid userId)
            : base(x => x.UserId == userId &&
            x.LoginProvider == UserTokenConstants.MangaBaseLoginProvider &&
            x.Name == UserTokenConstants.EmailVerificationTokenName)
        {
            AsTracking = true;
        }
    }
}
