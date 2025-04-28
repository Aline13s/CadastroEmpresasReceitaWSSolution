using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Tests.Mocks;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.EmpresaTests
{
    public class ListarEmpresasPorFiltroHandlerTests
    {
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly IMapper _mapper;
        private readonly ListarEmpresasFiltroHandler _handler;

        public ListarEmpresasPorFiltroHandlerTests()
        {
            _empresaRepoMock = new Mock<IEmpresaRepository>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _mapper = AutoMapperFactory.Create();

            _currentUserMock.Setup(u => u.ObterUsuarioId()).Returns("user-123");

            _handler = new ListarEmpresasFiltroHandler(
                _empresaRepoMock.Object,
                _currentUserMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Deve_Listar_Empresas_Por_Cnpj_E_NomeFantasia()
        {
            var empresas = new List<Empresa>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Cnpj = "12345678000100",
                    Nome = "Empresa Teste",
                    NomeFantasia = "Fantasia Teste",
                    Situacao = "ATIVA",
                    Tipo = "MATRIZ",
                    NaturezaJuridica = "Sociedade Limitada",
                    UsuarioId = "user-123",
                    Endereco = new(),
                    AtividadesPrincipais = new()
                }
            };

            var filtro = new ListarEmpresasFiltroDto { Cnpj = "123", NomeFantasia = "Fantasia" };
            var query = new ListarEmpresasFiltroQuery(filtro);

            _empresaRepoMock
                .Setup(r => r.BuscarAsync(It.IsAny<Expression<Func<Empresa, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(empresas);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1);
            result.First().Nome.Should().Be("Empresa Teste");
        }
    }
}
