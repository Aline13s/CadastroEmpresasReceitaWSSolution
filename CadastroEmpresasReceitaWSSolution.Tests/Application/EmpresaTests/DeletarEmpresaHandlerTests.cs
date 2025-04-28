using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using FluentAssertions;
using MediatR;
using Moq;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.EmpresaTests
{
    public class DeletarEmpresaHandlerTests
    {
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly DeletarEmpresaHandler _handler;

        public DeletarEmpresaHandlerTests()
        {
            _empresaRepoMock = new Mock<IEmpresaRepository>();
            _currentUserMock = new Mock<ICurrentUserService>();

            _currentUserMock.Setup(u => u.ObterUsuarioId()).Returns("user-123");

            _handler = new DeletarEmpresaHandler(
                _empresaRepoMock.Object,
                _currentUserMock.Object
            );
        }

        [Fact]
        public async Task Deve_Remover_Empresa_Se_Usuario_For_Dono()
        {
            // Arrange
            var empresa = new Empresa
            {
                Id = Guid.NewGuid(),
                UsuarioId = "user-123"
            };

            _empresaRepoMock.Setup(r => r.ObterPorIdAsync(empresa.Id, default))
                .ReturnsAsync(empresa);

            var command = new DeletarEmpresaCommand(empresa.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _empresaRepoMock.Verify(r => r.RemoverAsync(empresa.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_UnauthorizedAccessException_Se_Empresa_Nao_For_Do_Usuario()
        {
            // Arrange
            var empresa = new Empresa
            {
                Id = Guid.NewGuid(),
                UsuarioId = "outro-user"
            };

            _empresaRepoMock.Setup(r => r.ObterPorIdAsync(empresa.Id, default))
                .ReturnsAsync(empresa);

            var command = new DeletarEmpresaCommand(empresa.Id);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Empresa não encontrada ou sem permissão.");

            _empresaRepoMock.Verify(r => r.RemoverAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Lancar_UnauthorizedAccessException_Se_Empresa_Nao_Existir()
        {
            // Arrange
            var idEmpresa = Guid.NewGuid();

            _empresaRepoMock.Setup(r => r.ObterPorIdAsync(idEmpresa, default))
                .ReturnsAsync((Empresa?)null);

            var command = new DeletarEmpresaCommand(idEmpresa);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Empresa não encontrada ou sem permissão.");

            _empresaRepoMock.Verify(r => r.RemoverAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}