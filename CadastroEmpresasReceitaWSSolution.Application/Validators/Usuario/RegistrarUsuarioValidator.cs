using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using FluentValidation;

namespace CadastroEmpresasReceitaWSSolution.Application.Validators.Usuario
{
    public class RegistrarUsuarioValidator : AbstractValidator<RegistrarUsuarioDto>
    {
        public RegistrarUsuarioValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
        }
    }
}
