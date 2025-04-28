using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Domain.Entities
{
    public class Qsa
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Qual { get; set; }
        public string? PaisOrigem { get; set; }
        public string? NomeRepLegal { get; set; }
        public string? QualRepLegal { get; set; }
        public Guid EmpresaId { get; set; }
    }
}
