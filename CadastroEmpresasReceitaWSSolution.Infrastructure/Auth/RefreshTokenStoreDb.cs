using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Auth
{
    public class RefreshTokenStoreDb : IRefreshTokenStore
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenStoreDb(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(string userId, string refreshToken)
        {
            var existente = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.UsuarioId == userId);

            if (existente is not null)
            {
                existente.Token = refreshToken;
                existente.CriadoEm = DateTime.UtcNow;
                existente.ExpiraEm = DateTime.UtcNow.AddDays(7);
                _context.RefreshTokens.Update(existente);
            }
            else
            {
                var novo = new RefreshToken
                {
                    UsuarioId = userId,
                    Token = refreshToken,
                    CriadoEm = DateTime.UtcNow,
                    ExpiraEm = DateTime.UtcNow.AddDays(7)
                };
                await _context.RefreshTokens.AddAsync(novo);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidarAsync(string userId, string refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UsuarioId == userId && x.Token == refreshToken);

            if (token == null)
                return false;

            return token.ExpiraEm > DateTime.UtcNow;
        }

        public async Task RemoverAsync(string userId)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UsuarioId == userId);

            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverPorRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
