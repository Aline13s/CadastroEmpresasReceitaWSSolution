using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery
{
    public class ListarEmpresasFiltroQuery : IRequest<IEnumerable<EmpresaDto>>
    {
        public ListarEmpresasFiltroDto Filtro { get; }

        public ListarEmpresasFiltroQuery(ListarEmpresasFiltroDto filtro)
        {
            Filtro = filtro;
        }
    }
}
