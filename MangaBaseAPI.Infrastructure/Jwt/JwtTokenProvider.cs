using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MangaBaseAPI.Infrastructure.Jwt
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        const string JwtUserNameClaimType = "username";
        const string JwtExpirationClaimType = "exp";
        const string JwtRoleClaimType = "roles";

        readonly JwtOptions _jwtOptions;

        public JwtTokenProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public string GenerateAccessToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.NormalizedEmail!),
                new Claim(JwtUserNameClaimType, user.NormalizedUserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtRoleClaimType, role));
            }

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_jwtOptions.AccessTokenExpiryHours)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_jwtOptions.RefreshTokenExpiryDays)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsTokenInvalidOrExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(token))
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var expirationDateUnix = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtExpirationClaimType)?.Value;

                if (expirationDateUnix != null && long.TryParse(expirationDateUnix, out long expUnix))
                {
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                    return expirationDate < DateTime.UtcNow;
                }
            }
            // Token is invalid or missing expiration
            return true;
        }
    }
}
