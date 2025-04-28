using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.EmpresaHandler
{
    public class ListarEmpresasHandler : IRequestHandler<ListarEmpresasQuery, IEnumerable<EmpresaDto>>
    {
        private readonly IEmpresaRepository _repository;
        private readonly IMapper _mapper;

        public ListarEmpresasHandler(IEmpresaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmpresaDto>> Handle(ListarEmpresasQuery request, CancellationToken cancellationToken)
        {
            var empresas = await _repository.ObterTodosAsync(cancellationToken);

            if (empresas == null || !empresas.Any())
                throw new BusinessException("Nenhuma empresa foi encontrada.");

            return _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
        }
    }
}
