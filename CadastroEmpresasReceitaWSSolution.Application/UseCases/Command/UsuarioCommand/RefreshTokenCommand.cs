using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand
{
    public class RefreshTokenCommand : IRequest<AuthResult>
    {
        public TokenRefreshRequestDto RefreshRequest { get; }

        public RefreshTokenCommand(TokenRefreshRequestDto refreshRequest)
        {
            RefreshRequest = refreshRequest;
        }
    }
}
