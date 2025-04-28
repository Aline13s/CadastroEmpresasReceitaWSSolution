using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.UsuarioTests
{
    public class LogoutHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly LogoutHandler _handler;

        public LogoutHandlerTests()
        {
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _handler = new LogoutHandler(_refreshTokenRepositoryMock.Object);
        }

        [Fact]
        public async Task Deve_Deletar_RefreshToken_Valido()
        {
            // Arrange
            var refreshToken = "refresh-token-123";
            _refreshTokenRepositoryMock
                .Setup(r => r.ObterPorTokenAsync(refreshToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = refreshToken });

            // Act
            await _handler.Handle(new LogoutCommand(refreshToken), CancellationToken.None);

            // Assert
            _refreshTokenRepositoryMock.Verify(r => r.RemoverPorRefreshTokenAsync(refreshToken, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_RefreshToken_For_Invalido()
        {
            // Arrange
            var command = new LogoutCommand("");

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Refresh token inválido.");
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_RefreshToken_Nao_Encontrado()
        {
            // Arrange
            var refreshToken = "refresh-nao-existe";
            _refreshTokenRepositoryMock
                .Setup(r => r.ObterPorTokenAsync(refreshToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);

            var command = new LogoutCommand(refreshToken);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Refresh token não encontrado.");
        }
    }
}