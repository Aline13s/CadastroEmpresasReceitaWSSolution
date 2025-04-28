using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler
{
    public class AutenticarUsuarioHandler : IRequestHandler<AutenticarUsuarioCommand, AuthResult>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AutenticarUsuarioHandler(
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

        public async Task<AuthResult> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
        {
            if (request.Login == null)
                throw new ArgumentException("Dados de login são obrigatórios.", nameof(request.Login));

            if (string.IsNullOrWhiteSpace(request.Login.Email))
                throw new ArgumentException("O e-mail é obrigatório.", nameof(request.Login.Email));

            if (string.IsNullOrWhiteSpace(request.Login.Senha))
                throw new ArgumentException("A senha é obrigatória.", nameof(request.Login.Senha));

            var usuario = await _userManager.FindByEmailAsync(request.Login.Email);
            if (usuario == null || !await _userManager.CheckPasswordAsync(usuario, request.Login.Senha))
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            var token = _jwtService.GerarToken(usuario.Id, usuario.Email);
            var refresh = _jwtService.GerarRefreshToken();

            await _refreshTokenRepository.SalvarOuAtualizarAsync(usuario.Id, refresh, cancellationToken);

            return new AuthResult
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiracaoEmMinutos),
                RefreshToken = refresh
            };
        }
    }
}
