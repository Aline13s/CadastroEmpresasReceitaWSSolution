using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.Dtos
{
    public class AuthResult
    {
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
        public string RefreshToken { get; set; }
    }
}
