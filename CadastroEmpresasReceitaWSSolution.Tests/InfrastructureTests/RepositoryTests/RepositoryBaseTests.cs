using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.ValueObjects;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CadastroEmpresasReceitaWSSolution.Tests.InfrastructureTests.RepositoryTests
{
    public class RepositoryBaseTests
    {
        private readonly ApplicationDbContext _context;
        private readonly RepositoryBase<Empresa> _repository;

        public RepositoryBaseTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new RepositoryBase<Empresa>(_context);
        }

        [Fact]
        public async Task Deve_Adicionar_Empresa()
        {
            var empresa = CriarEmpresaCompleta("user-123", "Teste");

            await _repository.AdicionarAsync(empresa);
            var encontrada = await _context.Empresas.FindAsync(empresa.Id);

            encontrada.Should().NotBeNull();
            encontrada!.NomeFantasia.Should().Be("Teste");
        }

        [Fact]
        public async Task Deve_Atualizar_Empresa()
        {
            var empresa = CriarEmpresaCompleta();

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            empresa.NomeFantasia = "Atualizado";
            await _repository.AtualizarAsync(empresa);

            var atualizada = await _context.Empresas.FindAsync(empresa.Id);
            atualizada!.NomeFantasia.Should().Be("Atualizado");
        }

        [Fact]
        public async Task Deve_Remover_Empresa()
        {
            var empresa = CriarEmpresaCompleta();

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            await _repository.RemoverAsync(empresa.Id);
            var removida = await _context.Empresas.FindAsync(empresa.Id);

            removida.Should().BeNull();
        }

        [Fact]
        public async Task Deve_Obter_Todos()
        {
            _context.Empresas.AddRange(CriarEmpresaCompleta(), CriarEmpresaCompleta());
            await _context.SaveChangesAsync();

            var todos = await _repository.ObterTodosAsync();
            todos.Should().HaveCount(2);
        }

        [Fact]
        public async Task Deve_Verificar_Se_Existe()
        {
            var empresa = CriarEmpresaCompleta("user-123", "Exists");

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            var existe = await _repository.ExisteAsync(e => e.NomeFantasia == "Exists");
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
