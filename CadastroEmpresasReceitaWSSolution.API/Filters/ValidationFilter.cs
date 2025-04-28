using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CadastroEmpresasReceitaWSSolution.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => new
                    {
                        Campo = e.Key,
                        Erros = e.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                    })
                    .ToArray();

                var response = ApiResponse<object>.Fail("Ocorreram erros de validação.", errors);

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
