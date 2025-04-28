using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler
{
    public class AtualizarEmpresaHandler : IRequestHandler<AtualizarEmpresaCommand, Unit>
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IReceitaWsService _receitaWsService;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public AtualizarEmpresaHandler(
            IEmpresaRepository empresaRepository,
            IReceitaWsService receitaWsService,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _empresaRepository = empresaRepository;
            _receitaWsService = receitaWsService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AtualizarEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (request.Empresa == null)
                throw new BusinessException("Dados da empresa são obrigatórios.");

            var usuarioId = _currentUser.ObterUsuarioId();
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new BusinessException("Usuário não identificado.");

            var empresa = await _empresaRepository.ObterPorIdEUsuarioAsync(request.Empresa.Id, usuarioId, cancellationToken);
            if (empresa == null)
                throw new BusinessException("Empresa não encontrada ou sem permissão.");

            var response = await _receitaWsService.ConsultarCnpjAsync(empresa.Cnpj, cancellationToken);
            if (response == null || response.Status?.ToUpperInvariant() != "OK")
                throw new BusinessException("CNPJ inválido ou não encontrado na ReceitaWS.");

            _mapper.Map(response, empresa);

            await _empresaRepository.AtualizarAsync(empresa, cancellationToken);

            return Unit.Value;
        }
    }
}