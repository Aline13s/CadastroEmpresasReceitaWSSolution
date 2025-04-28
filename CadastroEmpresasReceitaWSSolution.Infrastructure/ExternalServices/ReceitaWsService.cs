using CadastroEmpresasReceitaWSSolution.Domain.Interfaces;
using CadastroEmpresasReceitaWSSolution.Domain.ExternalModels;
using System.Net.Http.Json;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.ExternalServices
{
    public class ReceitaWsService : IReceitaWsService
    {
        private readonly HttpClient _httpClient;

        public ReceitaWsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ReceitaWsResponse> ConsultarCnpjAsync(string cnpj, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<ReceitaWsResponse>(
                $"https://www.receitaws.com.br/v1/cnpj/{cnpj}",
                cancellationToken
            );

            return response ?? throw new Exception("Erro ao consultar CNPJ na ReceitaWS.");
        }
    }
}
