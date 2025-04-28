using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Domain.Interfaces
{
    public interface IRefreshTokenStore
    {
        Task SalvarAsync(string userId, string refreshToken);
        Task<bool> ValidarAsync(string userId, string refreshToken);
        Task RemoverAsync(string userId);
        Task RemoverPorRefreshTokenAsync(string refreshToken);
    }

}
