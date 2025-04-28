using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> ObterPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("UsuárioId inválido.", nameof(usuarioId));

            return await _dbSet.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId, cancellationToken);
        }

        public async Task<RefreshToken?> ObterPorTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token inválido.", nameof(refreshToken));

            return await _dbSet.FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);
        }

        public async Task SalvarOuAtualizarAsync(string usuarioId, string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("UsuárioId inválido.", nameof(usuarioId));
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token inválido.", nameof(refreshToken));

            var existente = await ObterPorUsuarioIdAsync(usuarioId, cancellationToken);

            if (existente is not null)
            {
                existente.Token = refreshToken;
                existente.CriadoEm = DateTime.UtcNow;
                existente.ExpiraEm = DateTime.UtcNow.AddDays(7);
                await AtualizarAsync(existente);
            }
            else
            {
                var novo = new RefreshToken
                {
                    UsuarioId = usuarioId,
                    Token = refreshToken,
                    CriadoEm = DateTime.UtcNow,
                    ExpiraEm = DateTime.UtcNow.AddDays(7)
                };
                await AdicionarAsync(novo);
            }
        }

        public async Task<bool> ValidarRefreshTokenAsync(string usuarioId, string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                return false;
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            var token = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.Token == refreshToken, cancellationToken);

            return token != null && token.ExpiraEm > DateTime.UtcNow;
        }

        public async Task RemoverPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("UsuárioId inválido.", nameof(usuarioId));

            var token = await ObterPorUsuarioIdAsync(usuarioId, cancellationToken);

            if (token is not null)
            {
                _dbSet.Remove(token);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RemoverPorRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token inválido.", nameof(refreshToken));

            var token = await ObterPorTokenAsync(refreshToken, cancellationToken);

            if (token is not null)
            {
                _dbSet.Remove(token);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
