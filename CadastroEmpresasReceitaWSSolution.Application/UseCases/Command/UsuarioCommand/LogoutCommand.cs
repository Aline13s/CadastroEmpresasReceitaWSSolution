using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand
{
    public record LogoutCommand(string RefreshToken) : IRequest<Unit>;
}
