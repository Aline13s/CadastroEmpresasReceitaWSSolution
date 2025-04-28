using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand
{
    public class DeletarEmpresaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeletarEmpresaCommand(Guid id) => Id = id;
    }
}
