using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace CadastroEmpresasReceitaWSSolution.Tests.InfrastructureTests.RepositoryTests
{
    public class UsuarioRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UsuarioRepository _repository;

        public UsuarioRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new UsuarioRepository(_context);
        }

        [Fact]
        public async Task Deve_Obter_Id_Por_Email()
        {
            var user = new Usuario { Id = "abc123", Email = "teste@email.com", UserName = "teste" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.ObterIdPorEmailAsync("teste@email.com");

            result.Should().Be("abc123");
        }
    }
}
