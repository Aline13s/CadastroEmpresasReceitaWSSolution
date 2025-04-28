using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.UsuarioTests
{
    public class AutenticarUsuarioHandlerTests
    {
        private readonly Mock<UserManager<Usuario>> _userManagerMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly IOptions<JwtSettings> _jwtOptions;
        private readonly AutenticarUsuarioHandler _handler;

        public AutenticarUsuarioHandlerTests()
        {
            var store = new Mock<IUserStore<Usuario>>();

            _userManagerMock = new Mock<UserManager<Usuario>>(store.Object, null, null, null, null, null, null, null, null);
            _jwtServiceMock = new Mock<IJwtService>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _jwtOptions = Options.Create(new JwtSettings { ExpiracaoEmMinutos = 120 });

            _handler = new AutenticarUsuarioHandler(
                _userManagerMock.Object,
                _jwtServiceMock.Object,
                _jwtOptions,
                _refreshTokenRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Deve_Autenticar_Usuario_Valido()
        {
            // Arrange
            var usuario = new Usuario { Id = "user-1", Email = "teste@teste.com" };
            var loginDto = new LoginDto { Email = "teste@teste.com", Senha = "senha123" };
            var command = new AutenticarUsuarioCommand(loginDto);

            _userManagerMock.Setup(m => m.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(usuario);

            _userManagerMock.Setup(m => m.CheckPasswordAsync(usuario, loginDto.Senha))
                .ReturnsAsync(true);

            _jwtServiceMock.Setup(j => j.GerarToken(usuario.Id, usuario.Email))
                .Returns("fake-jwt-token");

            _jwtServiceMock.Setup(j => j.GerarRefreshToken())
                .Returns("fake-refresh-token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-jwt-token");
            result.RefreshToken.Should().Be("fake-refresh-token");
            result.ExpiraEm.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(120), TimeSpan.FromSeconds(5));

            _refreshTokenRepositoryMock.Verify(r => r.SalvarOuAtualizarAsync(usuario.Id, "fake-refresh-token", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        public async Task Deve_Lancar_Unauthorized_Se_Email_Inexistente_Ou_Senha_Errada(object? senhaValida)
        {
            // Arrange
            var loginDto = new LoginDto { Email = "invalido@teste.com", Senha = "senhaerrada" };
            var command = new AutenticarUsuarioCommand(loginDto);

            if (senhaValida is null)
            {
                _userManagerMock.Setup(m => m.FindByEmailAsync(loginDto.Email))
                    .ReturnsAsync((Usuario?)null);
            }
            else
            {
                var fakeUsuario = new Usuario { Id = "user-fake", Email = loginDto.Email };

                _userManagerMock.Setup(m => m.FindByEmailAsync(loginDto.Email))
                    .ReturnsAsync(fakeUsuario);

                _userManagerMock.Setup(m => m.CheckPasswordAsync(fakeUsuario, loginDto.Senha))
                    .ReturnsAsync(false);
            }

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("E-mail ou senha inválidos.");
        }

        [Fact]
        public async Task Deve_Lancar_ArgumentException_Se_Login_Null()
        {
            // Arrange
            var command = new AutenticarUsuarioCommand(null!);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Dados de login são obrigatórios*");
        }

        [Fact]
        public async Task Deve_Lancar_ArgumentException_Se_Email_Vazio()
        {
            // Arrange
            var command = new AutenticarUsuarioCommand(new LoginDto { Email = "", Senha = "123456" });

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*O e-mail é obrigatório*");
        }

        [Fact]
        public async Task Deve_Lancar_ArgumentException_Se_Senha_Vazia()
        {
            // Arrange
            var command = new AutenticarUsuarioCommand(new LoginDto { Email = "teste@teste.com", Senha = "" });

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*A senha é obrigatória*");
        }
    }
}
