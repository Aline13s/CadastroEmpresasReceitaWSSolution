using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler
{
    public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, AuthResult>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RegistrarUsuarioHandler(
            UserManager<Usuario> userManager,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResult> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
        {
            if (request.Usuario == null)
                throw new BusinessException("Dados do usuário são obrigatórios.");

            if (string.IsNullOrWhiteSpace(request.Usuario.Email))
                throw new BusinessException("O e-mail é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Usuario.Senha))
                throw new BusinessException("A senha é obrigatória.");

            var novoUsuario = new Usuario
            {
                UserName = request.Usuario.Email,
                Email = request.Usuario.Email
            };

            var result = await _userManager.CreateAsync(novoUsuario, request.Usuario.Senha);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(TraduzirErro).ToList();
                throw new BusinessException(string.Join(" ", erros));
            }

            var token = _jwtService.GerarToken(novoUsuario.Id, novoUsuario.Email);
            var refresh = _jwtService.GerarRefreshToken();

            await _refreshTokenRepository.SalvarOuAtualizarAsync(novoUsuario.Id, refresh, cancellationToken);

            return new AuthResult
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiracaoEmMinutos),
                RefreshToken = refresh
            };
        }

        private static string TraduzirErro(IdentityError error)
        {
            return error.Code switch
            {
                "DuplicateUserName" => "Este e-mail já está sendo usado.",
                "DuplicateEmail" => "Este e-mail já está sendo usado.",
                "PasswordTooShort" => "A senha é muito curta.",
                "PasswordRequiresNonAlphanumeric" => "A senha deve conter pelo menos um caractere especial.",
                "PasswordRequiresLower" => "A senha deve conter pelo menos uma letra minúscula.",
                "PasswordRequiresUpper" => "A senha deve conter pelo menos uma letra maiúscula.",
                "PasswordRequiresDigit" => "A senha deve conter pelo menos um número.",
                _ => error.Description
            };
        }
    }
}
