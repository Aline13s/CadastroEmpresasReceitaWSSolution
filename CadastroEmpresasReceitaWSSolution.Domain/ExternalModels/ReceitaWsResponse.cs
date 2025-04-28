using System.Text.Json.Serialization;

namespace CadastroEmpresasReceitaWSSolution.Domain.ExternalModels
{
    public class ReceitaWsResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("ultima_atualizacao")]
        public string UltimaAtualizacao { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("porte")]
        public string Porte { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("fantasia")]
        public string Fantasia { get; set; }

        [JsonPropertyName("abertura")]
        public string Abertura { get; set; }

        [JsonPropertyName("atividade_principal")]
        public List<AtividadeReceitaWs> AtividadePrincipal { get; set; }

        [JsonPropertyName("atividades_secundarias")]
        public List<AtividadeReceitaWs> AtividadesSecundarias { get; set; }

        [JsonPropertyName("natureza_juridica")]
        public string NaturezaJuridica { get; set; }

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("municipio")]
        public string Municipio { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telefone")]
        public string Telefone { get; set; }

        [JsonPropertyName("efr")]
        public string Efr { get; set; }

        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        [JsonPropertyName("data_situacao")]
        public string DataSituacao { get; set; }

        [JsonPropertyName("motivo_situacao")]
        public string MotivoSituacao { get; set; }

        [JsonPropertyName("situacao_especial")]
        public string SituacaoEspecial { get; set; }

        [JsonPropertyName("data_situacao_especial")]
        public string DataSituacaoEspecial { get; set; }

        [JsonPropertyName("capital_social")]
        public string CapitalSocial { get; set; }

        [JsonPropertyName("qsa")]
        public List<QsaReceitaWs> Qsa { get; set; }

        [JsonPropertyName("simples")]
        public SimplesReceitaWs Simples { get; set; }

        [JsonPropertyName("simei")]
        public SimplesReceitaWs Simei { get; set; }

        [JsonPropertyName("billing")]
        public BillingReceitaWs Billing { get; set; }
    }

    public class AtividadeReceitaWs
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class QsaReceitaWs
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("qual")]
        public string Qual { get; set; }

        [JsonPropertyName("pais_origem")]
        public string PaisOrigem { get; set; }

        [JsonPropertyName("nome_rep_legal")]
        public string NomeRepLegal { get; set; }

        [JsonPropertyName("qual_rep_legal")]
        public string QualRepLegal { get; set; }
    }

    public class SimplesReceitaWs
    {
        [JsonPropertyName("optante")]
        public bool Optante { get; set; }

        [JsonPropertyName("data_opcao")]
        public string DataOpcao { get; set; }

        [JsonPropertyName("data_exclusao")]
        public string DataExclusao { get; set; }

        [JsonPropertyName("ultima_atualizacao")]
        public string UltimaAtualizacao { get; set; }
    }

    public class BillingReceitaWs
    {
        [JsonPropertyName("free")]
        public bool Free { get; set; }

        [JsonPropertyName("database")]
        public bool Database { get; set; }
    }
}