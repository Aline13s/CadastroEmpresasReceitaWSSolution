using MediatR;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand
{
    public class CadastrarEmpresaCommand : IRequest<Guid>
    {
        public CadastrarEmpresaDto Empresa { get; set; }

        public CadastrarEmpresaCommand(CadastrarEmpresaDto empresa)
        {
            Empresa = empresa;
        }
    }
}