using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
        public DateTime ExpiraEm { get; set; }
    }
}
