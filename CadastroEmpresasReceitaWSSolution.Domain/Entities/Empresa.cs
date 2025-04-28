using CadastroEmpresasReceitaWSSolution.Domain.ExternalModels;
using CadastroEmpresasReceitaWSSolution.Domain.ValueObjects;

namespace CadastroEmpresasReceitaWSSolution.Domain.Entities
{
    public partial class Empresa
    {
        public Guid Id { get; set; }
        public string Cnpj { get; set; } = null!; // CNPJ sempre obrigatório

        public string? Tipo { get; set; }
        public string? Porte { get; set; }
        public string? Nome { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Abertura { get; set; }
        public string? NaturezaJuridica { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Efr { get; set; }
        public string? Situacao { get; set; }
        public string? DataSituacao { get; set; }
        public string? MotivoSituacao { get; set; }
        public string? SituacaoEspecial { get; set; }
        public string? DataSituacaoEspecial { get; set; }
        public string? CapitalSocial { get; set; }
        public string UsuarioId { get; set; } = null!;
        public Endereco Endereco { get; set; } = null!;
        public List<AtividadePrincipal> AtividadesPrincipais { get; set; } = new();
        public List<AtividadeSecundaria> AtividadesSecundarias { get; set; } = new();
        public List<Qsa> Qsa { get; set; } = new();
        public Simples? Simples { get; set; }
        public Simei? Simei { get; set; }
        public Billing? Billing { get; set; }
    }
}
