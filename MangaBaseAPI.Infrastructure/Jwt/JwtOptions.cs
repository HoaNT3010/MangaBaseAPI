﻿namespace MangaBaseAPI.Infrastructure.Jwt
{
    public class JwtOptions
    {
        public string Issuer { get; init; } = string.Empty!;
        public string Audience { get; init; } = string.Empty!;
        public string SecretKey { get; init; } = string.Empty!;
        public int AccessTokenExpiryHours { get; init; }
        public int RefreshTokenExpiryDays { get; init; }
    }
}
