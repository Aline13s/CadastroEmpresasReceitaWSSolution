using AutoMapper;
using MediatR;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler
{
    public class CadastrarEmpresaHandler : IRequestHandler<CadastrarEmpresaCommand, Guid>
    {
        private readonly IReceitaWsService _receitaWsService;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public CadastrarEmpresaHandler(
            IReceitaWsService receitaWsService,
            IEmpresaRepository empresaRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _receitaWsService = receitaWsService;
            _empresaRepository = empresaRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CadastrarEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (request.Empresa == null)
                throw new BusinessException("Dados da empresa são obrigatórios.");

            if (string.IsNullOrWhiteSpace(request.Empresa.Cnpj))
                throw new BusinessException("O CNPJ é obrigatório.");

            var usuarioId = _currentUser.ObterUsuarioId();

            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new BusinessException("Usuário não identificado.");

            var existe = await _empresaRepository.EmpresaExistePorUsuarioAsync(request.Empresa.Cnpj, usuarioId, cancellationToken);
            if (existe)
                throw new BusinessException("Esta empresa já foi cadastrada por você.");

            var response = await _receitaWsService.ConsultarCnpjAsync(request.Empresa.Cnpj, cancellationToken);

            if (response == null || response.Status?.ToUpperInvariant() != "OK")
                throw new BusinessException("CNPJ inválido ou não encontrado na ReceitaWS.");

            var empresa = _mapper.Map<Empresa>(response);

            empresa.Id = Guid.NewGuid();
            empresa.UsuarioId = usuarioId;

            await _empresaRepository.AdicionarAsync(empresa, cancellationToken);

            return empresa.Id;
        }
    }
}
