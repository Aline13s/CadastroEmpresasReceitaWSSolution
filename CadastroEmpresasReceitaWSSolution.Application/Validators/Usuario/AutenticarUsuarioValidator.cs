using FluentValidation;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;

namespace CadastroEmpresasReceitaWSSolution.Application.Validators.Usuario
{
    public class AutenticarUsuarioValidator : AbstractValidator<LoginDto>
    {
        public AutenticarUsuarioValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
