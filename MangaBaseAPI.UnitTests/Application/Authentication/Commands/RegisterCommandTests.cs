using FluentAssertions;
using MangaBaseAPI.Application.Authentication.Commands.Register;
using MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Constants.Role;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MangaBaseAPI.UnitTests.Application.Helpers.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MangaBaseAPI.UnitTests.Application.Authentication.Commands
{
    public class RegisterCommandTests
    {
        private static readonly RegisterCommand Command = new(
            "user-name",
            "email",
            "password",
            "confirmed-password");

        private readonly UserManager<User> _userManagerMock;
        private readonly IPasswordHasher _passwordHasherMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly ILogger<RegisterCommandHandler> _loggerMock;
        private readonly IHangfireBackgroundJobService _jobServiceMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandTests()
        {
            _userManagerMock = MockUserManager.CreateMock<User>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _loggerMock = Substitute.For<ILogger<RegisterCommandHandler>>();
            _jobServiceMock = Substitute.For<IHangfireBackgroundJobService>();
            _handler = new RegisterCommandHandler(_userManagerMock,
                _passwordHasherMock,
                _unitOfWorkMock,
                _loggerMock,
                _jobServiceMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenEmailNotUnique()
        {
            // Arrange
            var invalidCommand = Command with { Email = "duplicated-email" };
            _userManagerMock.FindByEmailAsync(invalidCommand.Email)
                .Returns(Task.FromResult<User?>(new User()));

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RegisterErrors.EmailNotUnique);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserNameNotUnique()
        {
            // Arrange
            var invalidCommand = Command with { UserName = "duplicated-username" };
            _userManagerMock.FindByEmailAsync(invalidCommand.Email)
                .Returns(Task.FromResult<User?>(null));
            _userManagerMock.FindByNameAsync(invalidCommand.UserName)
                .Returns(Task.FromResult<User?>(new User()));

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RegisterErrors.UserNameNotUnique);
        }

        [Fact]
        public async Task Handle_Should_CreateNewUser_WhenRegisterSuccessful()
        {
            // Arrange
            _userManagerMock.FindByEmailAsync(Command.Email)
                .Returns(Task.FromResult<User?>(null));
            _userManagerMock.FindByNameAsync(Command.UserName)
                .Returns(Task.FromResult<User?>(null));
            User user = new User(Guid.NewGuid(),
                Command.UserName,
                Command.Email);
            _passwordHasherMock.HashProvidedPassword(Arg.Any<User>(), Command.Password)
                .Returns("hashed-password");
            _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);
            _userManagerMock.CreateAsync(Arg.Any<User>())
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMock.AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member)
                .Returns(Task.FromResult(IdentityResult.Success));
            _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeNull();
            await _userManagerMock.Received(1).CreateAsync(Arg.Any<User>());
            await _userManagerMock.Received(1).AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member);
            await _unitOfWorkMock.Received(0).RollbackTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenFailedToCreateUser()
        {
            // Arrange
            _userManagerMock.FindByEmailAsync(Command.Email)
                .Returns(Task.FromResult<User?>(null));
            _userManagerMock.FindByNameAsync(Command.UserName)
                .Returns(Task.FromResult<User?>(null));
            User user = new User(Guid.NewGuid(),
                Command.UserName,
                Command.Email);
            _passwordHasherMock.HashProvidedPassword(Arg.Any<User>(), Command.Password)
                .Returns("hashed-password");
            _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);
            _userManagerMock.CreateAsync(Arg.Any<User>())
                .Returns(Task.FromResult(IdentityResult.Failed()));

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RegisterErrors.CreateUserFailed);
            await _userManagerMock.Received(1).CreateAsync(Arg.Any<User>());
            await _userManagerMock.Received(0).AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member);
            await _unitOfWorkMock.Received(1).RollbackTransactionAsync(Arg.Any<CancellationToken>());
            await _unitOfWorkMock.Received(0).CommitTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenFailedToCreateUserRole()
        {
            // Arrange
            _userManagerMock.FindByEmailAsync(Command.Email)
                .Returns(Task.FromResult<User?>(null));
            _userManagerMock.FindByNameAsync(Command.UserName)
                .Returns(Task.FromResult<User?>(null));
            User user = new User(Guid.NewGuid(),
                Command.UserName,
                Command.Email);
            _passwordHasherMock.HashProvidedPassword(Arg.Any<User>(), Command.Password)
                .Returns("hashed-password");
            _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);
            _userManagerMock.CreateAsync(Arg.Any<User>())
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMock.AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member)
                .Returns(Task.FromResult(IdentityResult.Failed()));

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RegisterErrors.AssignUserRoleFailed);
            await _userManagerMock.Received(1).CreateAsync(Arg.Any<User>());
            await _userManagerMock.Received(1).AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member);
            await _unitOfWorkMock.Received(1).RollbackTransactionAsync(Arg.Any<CancellationToken>());
            await _unitOfWorkMock.Received(0).CommitTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenFailedToCommitTransaction()
        {
            // Arrange
            _userManagerMock.FindByEmailAsync(Command.Email)
                .Returns(Task.FromResult<User?>(null));
            _userManagerMock.FindByNameAsync(Command.UserName)
                .Returns(Task.FromResult<User?>(null));
            User user = new User(Guid.NewGuid(),
                Command.UserName,
                Command.Email);
            _passwordHasherMock.HashProvidedPassword(Arg.Any<User>(), Command.Password)
                .Returns("hashed-password");
            _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);
            _userManagerMock.CreateAsync(Arg.Any<User>())
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMock.AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member)
                .Returns(Task.FromResult(IdentityResult.Success));
            _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>())
                .ThrowsAsync(new Exception("Failed to commit database transaction"));

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RegisterErrors.UnexpectedError);
            await _userManagerMock.Received(1).CreateAsync(Arg.Any<User>());
            await _userManagerMock.Received(1).AddToRoleAsync(Arg.Any<User>(), ApplicationRoles.Member);
            await _unitOfWorkMock.Received(1).RollbackTransactionAsync(Arg.Any<CancellationToken>());
            await _unitOfWorkMock.Received(1).CommitTransactionAsync(Arg.Any<CancellationToken>());
        }
    }
}
