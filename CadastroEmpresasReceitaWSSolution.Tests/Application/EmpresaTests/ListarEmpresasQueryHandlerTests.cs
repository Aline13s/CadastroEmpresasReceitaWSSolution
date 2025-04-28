using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Tests.Mocks;
using FluentAssertions;
using Moq;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.EmpresaTests
{
    public class ListarEmpresasHandlerTests
    {
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly IMapper _mapper;
        private readonly ListarEmpresasHandler _handler;

        public ListarEmpresasHandlerTests()
        {
            _empresaRepoMock = new Mock<IEmpresaRepository>();
            _mapper = AutoMapperFactory.Create();

            _handler = new ListarEmpresasHandler(
                _empresaRepoMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Deve_Retornar_Todas_As_Empresas()
        {
            // Arrange
            var empresas = new List<Empresa>
            {
                new Empresa
                {
                    Id = Guid.NewGuid(),
                    Cnpj = "12345678000100",
                    Nome = "Empresa A",
                    NomeFantasia = "Fantasia A",
                    UsuarioId = "user-123",
                    Endereco = new(),
                    AtividadesPrincipais = new()
                },
                new Empresa
                {
                    Id = Guid.NewGuid(),
                    Cnpj = "98765432000100",
                    Nome = "Empresa B",
                    NomeFantasia = "Fantasia B",
                    UsuarioId = "user-456",
                    Endereco = new(),
                    AtividadesPrincipais = new()
                }
            };

            _empresaRepoMock
                .Setup(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(empresas);

            var query = new ListarEmpresasQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Nome.Should().Be("Empresa A");
            result.Last().Nome.Should().Be("Empresa B");
        }
    }
}