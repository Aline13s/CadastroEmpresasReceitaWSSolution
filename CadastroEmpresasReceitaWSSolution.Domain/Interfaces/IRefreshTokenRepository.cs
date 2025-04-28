using CadastroEmpresasReceitaWSSolution.Domain.Entities;

namespace CadastroEmpresasReceitaWSSolution.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        Task<RefreshToken?> ObterPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default);
        Task<RefreshToken?> ObterPorTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task SalvarOuAtualizarAsync(string usuarioId, string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> ValidarRefreshTokenAsync(string usuarioId, string refreshToken, CancellationToken cancellationToken = default);
        Task RemoverPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default);
        Task RemoverPorRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}