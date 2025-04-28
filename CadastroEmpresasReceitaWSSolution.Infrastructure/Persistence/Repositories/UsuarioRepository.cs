using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "O contexto não pode ser nulo.");
        }

        public async Task<string?> ObterIdPorEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("E-mail inválido.", nameof(email));

            return await _dbSet
                .Where(u => u.Email == email)
                .Select(u => u.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}