using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher _passwordHasher;

        public LoginQueryHandler(
            UserManager<User> userManager,
            IPasswordHasher passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<User>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.Failure<User>(LoginErrors.InvalidCredentials);
            }

            bool isCorrectPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
            if (!isCorrectPassword) 
            {
                return Result.Failure<User>(LoginErrors.InvalidCredentials);
            }
            return Result.SuccessNullError(user);
        }
    }
}
