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
using System.Threading;
using Xunit;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.EmpresaTests
{
    public class CadastrarEmpresaHandlerTests
    {
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly Mock<IReceitaWsService> _receitaWsServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly CadastrarEmpresaHandler _handler;

        public CadastrarEmpresaHandlerTests()
        {
            _empresaRepoMock = new Mock<IEmpresaRepository>();
            _receitaWsServiceMock = new Mock<IReceitaWsService>();
            _currentUserMock = new Mock<ICurrentUserService>();

            _currentUserMock.Setup(u => u.ObterUsuarioId()).Returns("user-123");

            _handler = new CadastrarEmpresaHandler(
                _receitaWsServiceMock.Object,
                _empresaRepoMock.Object,
                _currentUserMock.Object,
                AutoMapperFactory.Create()
            );
        }

        [Fact]
        public async Task Deve_Cadastrar_Empresa_Se_Nao_Existir()
        {
            // Arrange
            var dto = new CadastrarEmpresaDto { Cnpj = "12345678000100" };
            var command = new CadastrarEmpresaCommand(dto);

            _empresaRepoMock
                .Setup(r => r.EmpresaExistePorUsuarioAsync(dto.Cnpj, "user-123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _receitaWsServiceMock
                .Setup(r => r.ConsultarCnpjAsync(dto.Cnpj, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReceitaWsResponse
                {
                    Status = "OK",
                    Cnpj = dto.Cnpj,
                    Nome = "Empresa Teste",
                    Fantasia = "Fantasia Teste",
                    Situacao = "ATIVA",
                    Tipo = "MATRIZ",
                    Porte = "ME",
                    NaturezaJuridica = "Sociedade Limitada",
                    Logradouro = "Rua Teste",
                    Numero = "123",
                    Complemento = "Sala 1",
                    Bairro = "Centro",
                    Municipio = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Email = "contato@empresa.com",
                    Telefone = "11999999999",
                    Efr = "Não se aplica",
                    DataSituacao = "2022-01-01",
                    MotivoSituacao = "Nenhum",
                    SituacaoEspecial = "",
                    DataSituacaoEspecial = "",
                    CapitalSocial = "100000",
                    AtividadePrincipal = new() { new AtividadeReceitaWs { Code = "123", Text = "Atividade Teste" } },
                    AtividadesSecundarias = new(),
                    Qsa = new(),
                    Simples = new SimplesReceitaWs
                    {
                        Optante = true,
                        DataOpcao = "2020-01-01",
                        DataExclusao = null,
                        UltimaAtualizacao = "2023-01-01"
                    },
                    Simei = new SimplesReceitaWs
                    {
                        Optante = false,
                        DataOpcao = null,
                        DataExclusao = null,
                        UltimaAtualizacao = null
                    },
                    Billing = new BillingReceitaWs
                    {
                        Free = true,
                        Database = true
                    }
                });

            // Act
            var empresaId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            empresaId.Should().NotBeEmpty();
            _empresaRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Empresa>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Empresa_Ja_Existir()
        {
            // Arrange
            var dto = new CadastrarEmpresaDto { Cnpj = "12345678000100" };
            var command = new CadastrarEmpresaCommand(dto);

            _empresaRepoMock
                .Setup(r => r.EmpresaExistePorUsuarioAsync(dto.Cnpj, "user-123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Esta empresa já foi cadastrada por você.");
        }
    }
}