using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Domain.Interfaces
{
    public interface IEmpresaRepository : IRepositoryBase<Empresa>
    {
        Task<IEnumerable<Empresa>> ObterPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default);
        Task<Empresa?> ObterPorIdEUsuarioAsync(Guid id, string usuarioId, CancellationToken cancellationToken = default);
        Task<bool> EmpresaExisteAsync(string cnpj, CancellationToken cancellationToken = default);
        Task<bool> EmpresaExistePorUsuarioAsync(string cnpj, string usuarioId, CancellationToken cancellationToken = default);
    }
}
