using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
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
    public class ObterEmpresaPorIdHandler : IRequestHandler<ObterEmpresaPorIdQuery, EmpresaDto>
    {
        private readonly IEmpresaRepository _repository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public ObterEmpresaPorIdHandler(IEmpresaRepository repository, ICurrentUserService currentUser, IMapper mapper)
        {
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<EmpresaDto> Handle(ObterEmpresaPorIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Requisição inválida.");

            if (request.Id == Guid.Empty)
                throw new BusinessException("O ID da empresa é obrigatório.");

            var empresa = await _repository.ObterPorIdAsync(request.Id);

            if (empresa == null)
                throw new BusinessException("Nenhuma empresa foi encontrada com o Id informado.");

            return _mapper.Map<EmpresaDto>(empresa);
        }
    }

}
