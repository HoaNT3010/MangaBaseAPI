using FluentAssertions;
using MangaBaseAPI.Application.Authentication.Commands.RefreshToken;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Constants.User;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MangaBaseAPI.UnitTests.Application.Helpers.Services;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace MangaBaseAPI.UnitTests.Application.Authentication.Commands
{
    public class RefreshTokenCommandTests
    {
        private static readonly RefreshTokenCommand Command = new("valid-refresh-token");

        private readonly UserManager<User> _userManagerMock;
        private readonly IJwtTokenProvider _jwtTokenProviderMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly RefreshTokenCommandHandler _handler;
        private readonly IUserTokenRepository _userTokenRepoMock;

        public RefreshTokenCommandTests()
        {
            _userManagerMock = MockUserManager.CreateMock<User>();
            _jwtTokenProviderMock = Substitute.For<IJwtTokenProvider>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _userTokenRepoMock = Substitute.For<IUserTokenRepository>();
            _handler = new RefreshTokenCommandHandler(_userManagerMock, _jwtTokenProviderMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenTokenIsExpired()
        {
            // Arrange
            var invalidCommand = new RefreshTokenCommand(refreshToken: "expired-refresh-token");
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(invalidCommand.refreshToken)
                .Returns(true);

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RefreshTokenErrors.InvalidOrExpiredToken);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserIdNotFoundInToken()
        {
            // Arrange
            var invalidCommand = new RefreshTokenCommand(refreshToken: "refresh-token-with-no-user-id");
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(invalidCommand.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.GetUserIdByTokenValue(invalidCommand.refreshToken, CancellationToken.None)
                .Returns(Task.FromResult(Guid.Empty));

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RefreshTokenErrors.UserNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCannotRetrieveUserWithToken()
        {
            // Arrange
            var invalidCommand = new RefreshTokenCommand(refreshToken: "refresh-token-with-invalid-user-id");
            var userId = Guid.NewGuid();
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(invalidCommand.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.GetUserIdByTokenValue(invalidCommand.refreshToken, CancellationToken.None)
                .Returns(Task.FromResult(userId));
            _userManagerMock.FindByIdAsync(userId.ToString()).Returns(Task.FromResult<User?>(null));

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RefreshTokenErrors.UserNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenTokenUpdateFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(Command.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.GetUserIdByTokenValue(Command.refreshToken, CancellationToken.None)
                .Returns(Task.FromResult(userId));
            _userManagerMock.FindByIdAsync(userId.ToString()).Returns(Task.FromResult<User?>(user));
            _jwtTokenProviderMock.GenerateAccessToken(user, Arg.Any<IList<string>>())
                .Returns("new-access-token");
            _jwtTokenProviderMock.GenerateRefreshToken()
                .Returns("new-refresh-token");
            _userManagerMock.SetAuthenticationTokenAsync(user, UserTokenConstants.MangaBaseLoginProvider,
                    UserTokenConstants.JwtRefreshTokenName, "new-refresh-token")
                .Returns(IdentityResult.Failed());

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RefreshTokenErrors.UpdateRefreshTokenFailed);
        }

        [Fact]
        public async Task Handle_Should_ReturnNewTokens_WhenTokenUpdateSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(Command.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.GetUserIdByTokenValue(Command.refreshToken, CancellationToken.None)
                .Returns(Task.FromResult(userId));
            _userManagerMock.FindByIdAsync(userId.ToString()).Returns(Task.FromResult<User?>(user));
            _jwtTokenProviderMock.GenerateAccessToken(user, Arg.Any<IList<string>>())
                .Returns("new-access-token");
            _jwtTokenProviderMock.GenerateRefreshToken()
                .Returns("new-refresh-token");
            _userManagerMock.SetAuthenticationTokenAsync(user, UserTokenConstants.MangaBaseLoginProvider,
                    UserTokenConstants.JwtRefreshTokenName, "new-refresh-token")
                .Returns(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeNull();
            result.Value.AccessToken.Should().Be("new-access-token");
            result.Value.RefreshToken.Should().Be("new-refresh-token");
        }
    }
}
