using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.Dtos
{
    public class SimplesDto
    {
        public bool Optante { get; set; }
        public DateTime? DataOpcao { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
    }
}
