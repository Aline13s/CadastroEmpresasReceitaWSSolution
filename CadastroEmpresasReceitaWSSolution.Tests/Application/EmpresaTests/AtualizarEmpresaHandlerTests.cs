using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.ExternalModels;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Tests.Mocks;
using FluentAssertions;
using Moq;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.EmpresaTests
{
    public class AtualizarEmpresaHandlerTests
    {
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly Mock<IReceitaWsService> _receitaWsServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly IMapper _mapper;
        private readonly AtualizarEmpresaHandler _handler;

        public AtualizarEmpresaHandlerTests()
        {
            _empresaRepoMock = new Mock<IEmpresaRepository>();
            _receitaWsServiceMock = new Mock<IReceitaWsService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _mapper = AutoMapperFactory.Create();

            _currentUserMock.Setup(u => u.ObterUsuarioId()).Returns("user-123");

            _handler = new AtualizarEmpresaHandler(
                _empresaRepoMock.Object,
                _receitaWsServiceMock.Object,
                _currentUserMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Deve_Atualizar_Empresa_Se_CNPJ_Valido()
        {
            // Arrange
            var empresa = new Empresa
            {
                Id = Guid.NewGuid(),
                UsuarioId = "user-123",
                Cnpj = "12345678000100",
                NomeFantasia = "Empresa Antiga"
            };

            var receitaWsResponse = new ReceitaWsResponse
            {
                Status = "OK",
                Nome = "Empresa Atualizada",
                Fantasia = "Fantasia Atualizada",
                Cnpj = "12345678000100"
            };

            var dto = new AtualizarEmpresaDto { Id = empresa.Id };

            _empresaRepoMock
                .Setup(r => r.ObterPorIdEUsuarioAsync(empresa.Id, "user-123", default))
                .ReturnsAsync(empresa);

            _receitaWsServiceMock
                .Setup(r => r.ConsultarCnpjAsync(empresa.Cnpj, default))
                .ReturnsAsync(receitaWsResponse);

            // Act
            await _handler.Handle(new AtualizarEmpresaCommand(dto), CancellationToken.None);

            // Assert
            empresa.Nome.Should().Be("Empresa Atualizada");
            empresa.NomeFantasia.Should().Be("Fantasia Atualizada");

            _empresaRepoMock.Verify(r => r.AtualizarAsync(empresa, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Empresa_Nao_Encontrada()
        {
            // Arrange
            var idEmpresa = Guid.NewGuid();
            var dto = new AtualizarEmpresaDto { Id = idEmpresa };

            _empresaRepoMock
                .Setup(r => r.ObterPorIdEUsuarioAsync(idEmpresa, "user-123", default))
                .ReturnsAsync((Empresa?)null);

            // Act
            var act = async () => await _handler.Handle(new AtualizarEmpresaCommand(dto), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Empresa não encontrada ou sem permissão.");
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_CNPJ_Invalido_ReceitaWs()
        {
            // Arrange
            var empresa = new Empresa
            {
                Id = Guid.NewGuid(),
                UsuarioId = "user-123",
                Cnpj = "12345678000100"
            };

            var dto = new AtualizarEmpresaDto { Id = empresa.Id };

            _empresaRepoMock
                .Setup(r => r.ObterPorIdEUsuarioAsync(empresa.Id, "user-123", default))
                .ReturnsAsync(empresa);

            _receitaWsServiceMock
                .Setup(r => r.ConsultarCnpjAsync(empresa.Cnpj, default))
                .ReturnsAsync((ReceitaWsResponse?)null);

            // Act
            var act = async () => await _handler.Handle(new AtualizarEmpresaCommand(dto), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("CNPJ inválido ou não encontrado na ReceitaWS.");
        }
    }
}