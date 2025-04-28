using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand
{
    public class AutenticarUsuarioCommand : IRequest<AuthResult>
    {
        public LoginDto Login { get; set; }

        public AutenticarUsuarioCommand(LoginDto login)
        {
            Login = login;
        }
    }
}
