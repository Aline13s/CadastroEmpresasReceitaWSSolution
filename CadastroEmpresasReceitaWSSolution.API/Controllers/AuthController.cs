using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroEmpresasReceitaWSSolution.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var authResult = await _mediator.Send(new AutenticarUsuarioCommand(dto));
            return GerarRespostaComToken(authResult, "Login realizado com sucesso.");
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto dto)
        {
            var command = new RegistrarUsuarioCommand(dto);
            var authResult = await _mediator.Send(command);
            return GerarRespostaComToken(authResult, "Usuário registrado com sucesso.");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var tokenExpirado = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(tokenExpirado))
                return Unauthorized(ApiResponse<string>.Fail("Token expirado ausente."));

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<string>.Fail("Refresh token ausente."));
            
            var dto = new TokenRefreshRequestDto
            {
                TokenExpirado = tokenExpirado,
                RefreshToken = refreshToken
            };

            var authResult = await _mediator.Send(new RefreshTokenCommand(dto));
            return GerarRespostaComToken(authResult, "Token renovado com sucesso.");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<string>.Fail("Refresh token ausente. Não foi possível fazer logout."));

            if (!string.IsNullOrEmpty(refreshToken))
                await _mediator.Send(new LogoutCommand(refreshToken));

            Response.Cookies.Delete("refreshToken");

            return Ok(ApiResponse<string>.Ok("Logout realizado com sucesso."));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetPerfil()
        {
            var perfil = new
            {
                userId = _currentUserService.ObterUsuarioId(),
                email = _currentUserService.ObterEmail()
            };

            return Ok(ApiResponse<object>.Ok(perfil));
        }

        private IActionResult GerarRespostaComToken(AuthResult authResult, string mensagem)
        {
            Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true
            });

            var tokenDto = new TokenDto
            {
                Token = authResult.Token,
                ExpiraEm = authResult.ExpiraEm
            };

            return Ok(ApiResponse<TokenDto>.Ok(tokenDto, mensagem));
        }
    }
}
