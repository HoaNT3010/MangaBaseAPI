using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Role;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IPasswordHasher passwordHasher,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // Check if email is unique
            var isEmailUnique = await _userManager.FindByEmailAsync(request.Email);
            if (isEmailUnique != null)
            {
                return Result.Failure(RegisterErrors.EmailNotUnique);
            }

            // Check if user name is unique
            var isUserNameUnique = await _userManager.FindByNameAsync(request.UserName);
            if (isUserNameUnique != null)
            {
                return Result.Failure(RegisterErrors.UserNameNotUnique);
            }

            // Create user
            User newUser = new User(Guid.NewGuid(),
                request.UserName,
                request.Email);

            string hashedPassword = _passwordHasher.HashProvidedPassword(newUser, request.Password);
            newUser.SetInitialPassword(hashedPassword);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // Add user to db
                var addUserResult = await _userManager.CreateAsync(newUser);
                if (!addUserResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure(RegisterErrors.CreateUserFailed);
                }
                // Add user's role to db
                var addUserRoleResult = await _userManager.AddToRoleAsync(newUser, ApplicationRoles.Member);
                if (!addUserRoleResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure(RegisterErrors.AssignUserRoleFailed);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();

                // Need to implement logging!

                return Result.Failure(RegisterErrors.UnexpectedError);
            }

            return Result.SuccessNullError();
        }
    }
}
