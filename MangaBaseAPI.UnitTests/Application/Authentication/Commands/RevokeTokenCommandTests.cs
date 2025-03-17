using FluentAssertions;
using MangaBaseAPI.Application.Authentication.Commands.RevokeToken;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using NSubstitute;

namespace MangaBaseAPI.UnitTests.Application.Authentication.Commands
{
    public class RevokeTokenCommandTests
    {
        private static readonly RevokeTokenCommand Command = new("valid-refresh-token");

        private readonly IJwtTokenProvider _jwtTokenProviderMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IUserTokenRepository _userTokenRepoMock;
        private readonly RevokeTokenCommandHandler _handler;

        public RevokeTokenCommandTests()
        {
            _jwtTokenProviderMock = Substitute.For<IJwtTokenProvider>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _userTokenRepoMock = Substitute.For<IUserTokenRepository>();
            _handler = new RevokeTokenCommandHandler(_jwtTokenProviderMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenTokenIsExpired()
        {
            // Arrange
            var invalidCommand = new RevokeTokenCommand("expired-refresh-token");
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
        public async Task Handle_Should_ReturnError_WhenCannotFindAndRemoveTokenByValue()
        {
            // Arrange
            var invalidCommand = new RevokeTokenCommand("invalid-refresh-token");
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(invalidCommand.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.TryRemoveTokenByValueAsync(invalidCommand.refreshToken, CancellationToken.None)
                .Returns(false);

            // Act
            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RefreshTokenErrors.TokenEntryNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUpdateTokenFailed()
        {
            // Arrange
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(Command.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.TryRemoveTokenByValueAsync(Command.refreshToken, CancellationToken.None)
                .Returns(true);
            _unitOfWorkMock.SaveChangeAsync(CancellationToken.None).Returns(0);

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(RefreshTokenErrors.UpdateRefreshTokenFailed);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChange_WhenTokenIsValid()
        {
            // Arrange
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(Command.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.TryRemoveTokenByValueAsync(Command.refreshToken, CancellationToken.None)
                .Returns(true);

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            await _unitOfWorkMock.Received(1).SaveChangeAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_RevokeToken_WhenUpdateTokenSuccessful()
        {
            // Arrange
            _jwtTokenProviderMock.IsTokenInvalidOrExpired(Command.refreshToken)
                .Returns(false);
            _unitOfWorkMock.GetRepository<IUserTokenRepository>().Returns(_userTokenRepoMock);
            _userTokenRepoMock.TryRemoveTokenByValueAsync(Command.refreshToken, CancellationToken.None)
                .Returns(true);
            _unitOfWorkMock.SaveChangeAsync(CancellationToken.None).Returns(1);

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeNull();
            result.Value.message.Should().Be("Token revoked successfully");
        }
    }
}
