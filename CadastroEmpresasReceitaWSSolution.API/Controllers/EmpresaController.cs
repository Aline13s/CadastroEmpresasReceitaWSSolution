using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.EmpresaCommand;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Query.EmpresaQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroEmpresasReceitaWSSolution.API.Controllers
{
    [ApiController]
    [Route("api/empresas")]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmpresaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarEmpresaDto dto)
        {
            var id = await _mediator.Send(new CadastrarEmpresaCommand(dto));
            return Ok(ApiResponse<Guid>.Ok(id, "Empresa cadastrada com sucesso."));
        }

        [HttpGet]
        public async Task<IActionResult> ListarEmpresas()
        {
            var empresas = await _mediator.Send(new ListarEmpresasQuery());
            return Ok(ApiResponse<IEnumerable<EmpresaDto>>.Ok(empresas));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var empresa = await _mediator.Send(new ObterEmpresaPorIdQuery(id));
            return Ok(ApiResponse<EmpresaDto>.Ok(empresa));
        }

        [HttpGet("filtro")]
        public async Task<IActionResult> ListarEmpresasFiltro([FromQuery] ListarEmpresasFiltroDto filtroDto)
        {
            var empresas = await _mediator.Send(new ListarEmpresasFiltroQuery(filtroDto));
            return Ok(ApiResponse<IEnumerable<EmpresaDto>>.Ok(empresas));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarEmpresaDto dto)
        {
            dto.Id = id;
            await _mediator.Send(new AtualizarEmpresaCommand(dto));
            return Ok(ApiResponse<string>.Ok("Empresa atualizada com sucesso."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _mediator.Send(new DeletarEmpresaCommand(id));
            return Ok(ApiResponse<string>.Ok("Empresa removida com sucesso."));
        }
    }
}
