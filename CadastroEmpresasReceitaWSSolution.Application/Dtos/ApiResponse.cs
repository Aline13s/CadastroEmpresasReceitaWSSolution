namespace CadastroEmpresasReceitaWSSolution.Application.Dtos
{
    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T Dados { get; set; }
        public object Erros { get; set; }

        public static ApiResponse<T> Ok(T dados, string mensagem = "Operação realizada com sucesso.")
        {
            return new ApiResponse<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados,
                Erros = null
            };
        }

        public static ApiResponse<T> Fail(string mensagem, object erros = null)
        {
            return new ApiResponse<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Dados = default,
                Erros = erros
            };
        }
    }
}
