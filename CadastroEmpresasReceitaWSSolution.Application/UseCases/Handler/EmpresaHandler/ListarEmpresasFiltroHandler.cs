using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
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
    public class ListarEmpresasFiltroHandler : IRequestHandler<ListarEmpresasFiltroQuery, IEnumerable<EmpresaDto>>
    {
        private readonly IEmpresaRepository _repository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public ListarEmpresasFiltroHandler(
            IEmpresaRepository repository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmpresaDto>> Handle(ListarEmpresasFiltroQuery request, CancellationToken cancellationToken)
        {
            var usuarioId = _currentUser.ObterUsuarioId();
            var pesquisaPorUsuario = request.Filtro.PesquisaPorIdUsuario;

            if (pesquisaPorUsuario && string.IsNullOrWhiteSpace(usuarioId))
                throw new UnauthorizedAccessException("Usuário não identificado para realizar a pesquisa.");

            var empresas = await _repository.BuscarAsync(e =>
                (!pesquisaPorUsuario || e.UsuarioId == usuarioId) &&
                (string.IsNullOrEmpty(request.Filtro.Cnpj) || e.Cnpj.Contains(request.Filtro.Cnpj)) &&
                (string.IsNullOrEmpty(request.Filtro.NomeFantasia) || e.NomeFantasia.Contains(request.Filtro.NomeFantasia)),
                cancellationToken
            );

            if (empresas == null || !empresas.Any())
                throw new BusinessException("Nenhuma empresa foi encontrada com os filtros informados.");

            return _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
        }
    }
}
