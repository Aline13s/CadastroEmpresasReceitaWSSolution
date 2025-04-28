using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "O contexto não pode ser nulo.");
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("O id informado é inválido.", nameof(id));

            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "A expressão de busca não pode ser nula.");

            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> ExisteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "A expressão de verificação não pode ser nula.");

            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task AdicionarAsync(TEntity entidade, CancellationToken cancellationToken = default)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade), "A entidade para adicionar não pode ser nula.");

            await _dbSet.AddAsync(entidade, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task AtualizarAsync(TEntity entidade, CancellationToken cancellationToken = default)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade), "A entidade para atualizar não pode ser nula.");

            _dbSet.Update(entidade);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("O id informado é inválido.", nameof(id));

            var entidade = await ObterPorIdAsync(id, cancellationToken);
            if (entidade != null)
            {
                _dbSet.Remove(entidade);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}