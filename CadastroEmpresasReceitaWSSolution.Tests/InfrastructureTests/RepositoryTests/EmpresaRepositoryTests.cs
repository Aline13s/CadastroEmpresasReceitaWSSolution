using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.ValueObjects;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CadastroEmpresasReceitaWSSolution.Tests.InfrastructureTests.RepositoryTests
{
    public class EmpresaRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly EmpresaRepository _repository;

        public EmpresaRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new EmpresaRepository(_context);
        }

        [Fact]
        public async Task Deve_Obter_Empresas_Por_UsuarioId()
        {
            // Arrange
            var empresa1 = CriarEmpresaCompleta("user1", "Empresa 1");
            var empresa2 = CriarEmpresaCompleta("user2", "Empresa 2");

            _context.Empresas.AddRange(empresa1, empresa2);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repository.ObterPorUsuarioIdAsync("user1");

            // Assert
            resultado.Should().ContainSingle(e => e.UsuarioId == "user1");
        }

        [Fact]
        public async Task Deve_Verificar_Se_Empresa_Existe_Por_Cnpj()
        {
            // Arrange
            var empresa = CriarEmpresaCompleta("user1", "Empresa 1");
            empresa.Cnpj = "12345678901234";

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            // Act
            var existe = await _repository.EmpresaExisteAsync("12345678901234");

            // Assert
            existe.Should().BeTrue();
        }

        [Fact]
        public async Task Deve_Verificar_Se_Empresa_Existe_Por_Cnpj_E_Usuario()
        {
            // Arrange
            var empresa = CriarEmpresaCompleta("user1", "Empresa 1");
            empresa.Cnpj = "12345678901234";

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            // Act
            var existe = await _repository.EmpresaExistePorUsuarioAsync("12345678901234", "user1");

            // Assert
            existe.Should().BeTrue();
        }

        private Empresa CriarEmpresaCompleta(string usuarioId = "user-123", string nomeFantasia = "Fake")
        {
            return new Empresa
            {
                Id = Guid.NewGuid(),
                Cnpj = "12345678000100",
                Tipo = "MATRIZ",
                Porte = "ME",
                Nome = "Empresa Exemplo LTDA",
                NomeFantasia = nomeFantasia,
                Abertura = "2020-01-01",
                NaturezaJuridica = "Sociedade Empresária Limitada",
                Email = "teste@empresa.com",
                Telefone = "11999999999",
                Efr = "Não se aplica",
                Situacao = "ATIVA",
                DataSituacao = "2022-01-01",
                MotivoSituacao = "OK",
                SituacaoEspecial = "",
                DataSituacaoEspecial = "",
                CapitalSocial = "100000.00",
                UsuarioId = usuarioId,
                Endereco = new Endereco
                {
                    Logradouro = "Rua de Teste",
                    Numero = "123",
                    Complemento = "Sala 1",
                    Bairro = "Centro",
                    Municipio = "São Paulo",
                    Uf = "SP",
                    Cep = "01234567"
                },
                AtividadesPrincipais = new List<AtividadePrincipal>
                {
                    new AtividadePrincipal
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "6201-5/01",
                        Descricao = "Desenvolvimento de software sob encomenda"
                    }
                },
                AtividadesSecundarias = new List<AtividadeSecundaria>
                {
                    new AtividadeSecundaria
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "6209-1/00",
                        Descricao = "Suporte técnico"
                    }
                },
                Qsa = new List<Qsa>
                {
                    new Qsa
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Fulano de Tal",
                        Qual = "Sócio-Administrador",
                        PaisOrigem = "Brasil",
                        NomeRepLegal = "",
                        QualRepLegal = ""
                    }
                },
                Simples = new Simples
                {
                    Optante = true,
                    DataOpcao = new DateTime(2021, 01, 01),
                    DataExclusao = null,
                    UltimaAtualizacao = DateTime.UtcNow
                },
                Simei = new Simei
                {
                    Optante = false,
                    DataOpcao = null,
                    DataExclusao = null,
                    UltimaAtualizacao = DateTime.UtcNow
                },
                Billing = new Billing
                {
                    Free = true,
                    Database = true
                }
            };
        }
    }
}