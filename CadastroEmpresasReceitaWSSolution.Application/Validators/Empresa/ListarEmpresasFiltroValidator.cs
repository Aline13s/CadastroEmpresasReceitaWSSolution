using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using FluentValidation;
using System.Linq;

namespace CadastroEmpresasReceitaWSSolution.Application.Validators.Empresa
{
    public class ListarEmpresasFiltroValidator : AbstractValidator<ListarEmpresasFiltroDto>
    {
        public ListarEmpresasFiltroValidator()
        {
            When(x => !string.IsNullOrWhiteSpace(x.Cnpj), () =>
            {
                RuleFor(x => x.Cnpj)
                    .Must(ValidarCnpj)
                    .WithMessage("O CNPJ informado no filtro é inválido.");
            });
        }

        private bool ValidarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            return cnpj.Length == 14;
        }
    }
}