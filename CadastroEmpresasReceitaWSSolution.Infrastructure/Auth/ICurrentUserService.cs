using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Auth
{
    public interface ICurrentUserService
    {
        string? ObterUsuarioId();
        string? ObterEmail();
    }
}
