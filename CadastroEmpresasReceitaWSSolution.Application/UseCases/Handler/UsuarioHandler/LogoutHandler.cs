using CadastroEmpresasReceitaWSSolution.Application.Exceptions;
using CadastroEmpresasReceitaWSSolution.Application.UseCases.Command.UsuarioCommand;
using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using MediatR;

namespace CadastroEmpresasReceitaWSSolution.Application.UseCases.Handler.UsuarioHandler
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new BusinessException("Refresh token inválido.");

            var refreshToken = await _refreshTokenRepository.ObterPorTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken == null)
                throw new BusinessException("Refresh token não encontrado.");

            await _refreshTokenRepository.RemoverPorRefreshTokenAsync(request.RefreshToken, cancellationToken);
            return Unit.Value;
        }
    }
}
