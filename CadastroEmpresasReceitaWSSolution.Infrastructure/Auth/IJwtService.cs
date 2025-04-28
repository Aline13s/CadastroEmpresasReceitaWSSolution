using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Auth
{
    public interface IJwtService
    {
        string GerarToken(string usuarioId, string email);
        string GerarRefreshToken();
        ClaimsPrincipal? ObterClaimsDoTokenExpirado(string token);
    }

}
