using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<string?> ObterIdPorEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
