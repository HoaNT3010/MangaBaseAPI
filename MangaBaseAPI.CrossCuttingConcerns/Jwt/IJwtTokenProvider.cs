using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.CrossCuttingConcerns.Jwt
{
    public interface IJwtTokenProvider
    {
        public string GenerateAccessToken(User user, IList<string> roles);
        public string GenerateRefreshToken();
        public bool IsTokenInvalidOrExpired(string token);
    }
}
