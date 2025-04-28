using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.Dtos
{
    public class ListarEmpresasFiltroDto
    {
        public string? NomeFantasia { get; set; }
        public string? Cnpj { get; set; }
        public bool PesquisaPorIdUsuario { get; set; } = false;
    }
}
