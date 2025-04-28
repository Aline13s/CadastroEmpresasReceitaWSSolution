using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.Dtos
{
    public class TokenRefreshRequestDto
    {
        public string TokenExpirado { get; set; }
        public string RefreshToken { get; set; }
    }
}
