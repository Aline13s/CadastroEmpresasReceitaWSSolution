using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand
{
    public class RegistrarUsuarioCommand : IRequest<AuthResult>
    {
        public RegistrarUsuarioDto Usuario { get; set; }

        public RegistrarUsuarioCommand(RegistrarUsuarioDto usuario)
        {
            Usuario = usuario;
        }
    }
}
