using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand
{
    public class AtualizarEmpresaCommand : IRequest<Unit>
    {
        public AtualizarEmpresaDto Empresa { get; set; }

        public AtualizarEmpresaCommand(AtualizarEmpresaDto empresa)
        {
            Empresa = empresa;
        }
    }
}
