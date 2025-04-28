using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> ObterTodosAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> ExisteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task AdicionarAsync(TEntity entidade, CancellationToken cancellationToken = default);
        Task AtualizarAsync(TEntity entidade, CancellationToken cancellationToken = default);
        Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
