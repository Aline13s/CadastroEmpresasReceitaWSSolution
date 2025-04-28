using System.Threading.Tasks;
using CadastroEmpresasReceitaWSSolution.Domain.ExternalModels;

namespace CadastroEmpresasReceitaWSSolution.Domain.Interfaces
{
    public interface IReceitaWsService
    {
        Task<ReceitaWsResponse?> ConsultarCnpjAsync(string cnpj, CancellationToken cancellationToken);
    }
}
