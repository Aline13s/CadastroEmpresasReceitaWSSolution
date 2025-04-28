using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery
{
    public class ObterEmpresaPorIdQuery : IRequest<EmpresaDto>
    {
        public Guid Id { get; set; }

        public ObterEmpresaPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
