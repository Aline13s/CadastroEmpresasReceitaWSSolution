using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler
{
    public class DeletarEmpresaHandler : IRequestHandler<DeletarEmpresaCommand, Unit>
    {
        private readonly IEmpresaRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeletarEmpresaHandler(IEmpresaRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeletarEmpresaCommand request, CancellationToken cancellationToken)
        {
            var empresa = await _repository.ObterPorIdAsync(request.Id);
            if (empresa == null || empresa.UsuarioId != _currentUser.ObterUsuarioId())
                throw new UnauthorizedAccessException("Empresa não encontrada ou sem permissão.");

            await _repository.RemoverAsync(request.Id);
            return Unit.Value;
        }
    }
}
