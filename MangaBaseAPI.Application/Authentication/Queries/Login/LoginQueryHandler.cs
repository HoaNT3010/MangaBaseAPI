using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, User?>
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

        public async Task<User?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                Console.WriteLine("Cannot find user with email");
                return null; 
            }

            bool isCorrectPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
            if (!isCorrectPassword) 
            {
                Console.WriteLine("Wrong password!");
                return null;
            }
            return user;
        }
    }
}
