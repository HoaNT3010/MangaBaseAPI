using FluentAssertions;
using MangaBaseAPI.Application.Authentication.Commands.Login;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.UnitTests.Application.Helpers.Services;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace MangaBaseAPI.UnitTests.Application.Authentication.Commands
{
    public class LoginCommandTests
    {
        private static readonly LoginCommand Command = new("test@test.com", "123456");

        private readonly LoginCommandHandler _handler;
        private readonly UserManager<User> _userManagerMock;
        private readonly IPasswordHasher _passwordHasherMock;
        private readonly IJwtTokenProvider _jwtTokenProviderMock;

        public LoginCommandTests()
        {
            _userManagerMock = MockUserManager.CreateMock<User>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _jwtTokenProviderMock = Substitute.For<IJwtTokenProvider>();

            _handler = new LoginCommandHandler(_userManagerMock, _passwordHasherMock, _jwtTokenProviderMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var invalidCommand = Command with { Email = "test1@test.com" };
            _userManagerMock.FindByEmailAsync(Arg.Is<string>(e => e != Command.Email))
                .Returns(Task.FromResult<User?>(null));

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(LoginErrors.InvalidCredentials);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User { Email = "test@test.com", PasswordHash = "hashed-password" };
            var invalidCommand = new LoginCommand(Email: "test@test.com", Password: "wrong-password");
            _userManagerMock.FindByEmailAsync(invalidCommand.Email)
                .Returns(Task.FromResult<User?>(user));
            _passwordHasherMock.VerifyHashedPassword(user, user.PasswordHash, invalidCommand.Password)
                .Returns(false);

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(LoginErrors.InvalidCredentials);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenTokenUpdateFails()
        {
            // Arrange
            var user = new User { Email = "test@test.com", PasswordHash = "hashed-password" };
            _userManagerMock.FindByEmailAsync(Arg.Is<string>(e => e == user.Email))
                .Returns(Task.FromResult<User?>(user));
            _passwordHasherMock.VerifyHashedPassword(user, user.PasswordHash, Command.Password)
                .Returns(true);
            _userManagerMock.GetRolesAsync(user).Returns(Task.FromResult<IList<string>>(new List<string> { "Member" }));
            _jwtTokenProviderMock.GenerateAccessToken(user, Arg.Any<IList<string>>()).Returns("mocked-access-token");
            _jwtTokenProviderMock.GenerateRefreshToken().Returns("mocked-refresh-token");

            _userManagerMock.SetAuthenticationTokenAsync(user, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Failed()));
            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(LoginErrors.UpdateRefreshTokenFailed);
        }

        [Fact]
        public async Task Handle_Should_LoginSuccessful_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User { Email = "test@test.com", PasswordHash = "hashed-password" };
            _userManagerMock.FindByEmailAsync(Arg.Is<string>(e => e == user.Email))
                .Returns(Task.FromResult<User?>(user));
            _passwordHasherMock.VerifyHashedPassword(user, user.PasswordHash, Arg.Is<string>(e => e == Command.Password))
                .Returns(true);
            _userManagerMock.GetRolesAsync(user).Returns(Task.FromResult<IList<string>>(new List<string> { "Member" }));
            _jwtTokenProviderMock.GenerateAccessToken(user, Arg.Any<IList<string>>()).Returns("mocked-access-token");
            _jwtTokenProviderMock.GenerateRefreshToken().Returns("mocked-refresh-token");
            _userManagerMock.SetAuthenticationTokenAsync(user, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Success));

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeNull();
            result.Value.AccessToken.Should().Be("mocked-access-token");
            result.Value.RefreshToken.Should().Be("mocked-refresh-token");
        }
    }
}
