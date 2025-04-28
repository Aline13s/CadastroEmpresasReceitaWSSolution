using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpresaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Empresa?> ObterPorIdEUsuarioAsync(Guid id, string usuarioId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id da empresa inválido.", nameof(id));

            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("UsuárioId inválido.", nameof(usuarioId));

            return await _dbSet.Where(e => e.Id == id && e.UsuarioId == usuarioId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Empresa>> ObterPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("UsuárioId inválido.", nameof(usuarioId));

            return await _dbSet
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> EmpresaExisteAsync(string cnpj, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ inválido.", nameof(cnpj));

            return await _dbSet
                .AnyAsync(e => e.Cnpj == cnpj, cancellationToken);
        }

        public async Task<bool> EmpresaExistePorUsuarioAsync(string cnpj, string usuarioId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ inválido.", nameof(cnpj));

            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("Usuario inválido.", nameof(usuarioId));

            return await _dbSet
                .AnyAsync(e => e.Cnpj == cnpj && e.UsuarioId == usuarioId, cancellationToken);
        }
    }
}
