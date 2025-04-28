using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using FluentAssertions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.UsuarioTests
{
    public class RegistrarUsuarioHandlerTests
    {
        private readonly Mock<UserManager<Usuario>> _userManagerMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IRefreshTokenRepository> _refreshStoreMock;
        private readonly IOptions<JwtSettings> _jwtOptions;

        private readonly RegistrarUsuarioHandler _handler;

        public RegistrarUsuarioHandlerTests()
        {
            _userManagerMock = MockUserManager.Create<Usuario>();
            _jwtServiceMock = new Mock<IJwtService>();
            _refreshStoreMock = new Mock<IRefreshTokenRepository>();
            _jwtOptions = Options.Create(new JwtSettings
            {
                Secret = "segredo-muito-seguro-aqui1234567890",
                Audience = "test-audience",
                Issuer = "test-issuer",
                ExpiracaoEmMinutos = 5
            });

            _handler = new RegistrarUsuarioHandler(_userManagerMock.Object, _jwtServiceMock.Object, _jwtOptions, _refreshStoreMock.Object);
        }

        [Fact]
        public async Task Deve_Registrar_Usuario_E_Retornar_Token()
        {
            var dto = new RegistrarUsuarioDto { Email = "user@test.com", Senha = "Senha123!" };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Usuario>(), dto.Senha))
                            .ReturnsAsync(IdentityResult.Success);

            _jwtServiceMock.Setup(x => x.GerarToken(It.IsAny<string>(), dto.Email)).Returns("fake-token");
            _jwtServiceMock.Setup(x => x.GerarRefreshToken()).Returns("fake-refresh-token");

            var result = await _handler.Handle(new RegistrarUsuarioCommand(dto), CancellationToken.None);

            result.Token.Should().Be("fake-token");
            result.RefreshToken.Should().Be("fake-refresh-token");
        }
    }
}
