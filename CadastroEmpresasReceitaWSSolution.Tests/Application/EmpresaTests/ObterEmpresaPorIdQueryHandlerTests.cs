using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Tests.Mocks;
using FluentAssertions;
using Moq;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.EmpresaTests
{
    public class ObterEmpresaPorIdHandlerTests
    {
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly IMapper _mapper;
        private readonly ObterEmpresaPorIdHandler _handler;

        public ObterEmpresaPorIdHandlerTests()
        {
            _empresaRepoMock = new Mock<IEmpresaRepository>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _mapper = AutoMapperFactory.Create();

            _handler = new ObterEmpresaPorIdHandler(
                _empresaRepoMock.Object,
                _currentUserMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Deve_Retornar_Empresa_Se_Existir()
        {
            // Arrange
            var id = Guid.NewGuid();
            var empresa = new Empresa
            {
                Id = id,
                Cnpj = "12345678000100",
                Nome = "Empresa Teste",
                UsuarioId = "user-123"
            };

            _empresaRepoMock.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(empresa);

            // Act
            var resultado = await _handler.Handle(new ObterEmpresaPorIdQuery(id), CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(id);
            resultado.Nome.Should().Be("Empresa Teste");
        }

        [Fact]
        public async Task Deve_Lancar_BusinessException_Se_Empresa_Nao_Existir()
        {
            // Arrange
            var id = Guid.NewGuid();
            _empresaRepoMock.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Empresa?)null);

            // Act
            var act = async () => await _handler.Handle(new ObterEmpresaPorIdQuery(id), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Nenhuma empresa foi encontrada com o Id informado.");
        }

        [Fact]
        public async Task Deve_Lancar_BusinessException_Se_Id_For_Empty()
        {
            // Arrange
            var query = new ObterEmpresaPorIdQuery(Guid.Empty);

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("O ID da empresa é obrigatório.");
        }

        [Fact]
        public async Task Deve_Lancar_ArgumentNullException_Se_Request_For_Nulo()
        {
            // Act
            var act = async () => await _handler.Handle(null!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Requisição inválida*");
        }
    }
}
