using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using FluentValidation;

namespace CadastroEmpresasReceitaWSSolution.Application.Validators.Empresa
{
    public class CadastrarEmpresaValidator : AbstractValidator<CadastrarEmpresaDto>
    {
        public CadastrarEmpresaValidator()
        {
            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Must(ValidarCnpj).WithMessage("O CNPJ informado é inválido.");
        }

        private bool ValidarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return false;

            return true;
        }
    }
}