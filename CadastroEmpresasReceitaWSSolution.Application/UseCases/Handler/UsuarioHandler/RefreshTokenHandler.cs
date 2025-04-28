using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshRepository;
        private readonly UserManager<CadastroEmpresasReceitaWSSolution.Infrastructure.Identity.Usuario> _userManager;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenHandler(IJwtService jwtService, IRefreshTokenRepository refreshRepository, UserManager<Usuario> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _jwtService = jwtService;
            _refreshRepository = refreshRepository;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public UserManager<Infrastructure.Identity.Usuario> UserManager => _userManager;

        public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.RefreshRequest == null)
                throw new SecurityTokenException("Dados para renovação de token são obrigatórios.");

            if (string.IsNullOrWhiteSpace(request.RefreshRequest.TokenExpirado))
                throw new SecurityTokenException("Token expirado é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.RefreshRequest.RefreshToken))
                throw new SecurityTokenException("Refresh token é obrigatório.");

            var principal = _jwtService.ObterClaimsDoTokenExpirado(request.RefreshRequest.TokenExpirado);
            if (principal == null)
                throw new SecurityTokenException("Token inválido.");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email))
                throw new SecurityTokenException("Informações do usuário no token são inválidas.");

            var valido = await _refreshRepository.ValidarRefreshTokenAsync(userId, request.RefreshRequest.RefreshToken, cancellationToken);
            if (!valido)
                throw new SecurityTokenException("Refresh token inválido.");

            var token = _jwtService.GerarToken(userId, email);
            var refresh = _jwtService.GerarRefreshToken();

            await _refreshRepository.SalvarOuAtualizarAsync(userId, refresh, cancellationToken);

            return new AuthResult
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiracaoEmMinutos),
                RefreshToken = refresh
            };
        }
    }
}
