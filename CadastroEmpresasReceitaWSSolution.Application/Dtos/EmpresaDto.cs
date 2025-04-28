using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;

public class EmpresaDto
{
    public Guid Id { get; set; }
    public string Cnpj { get; set; }
    public string Tipo { get; set; }
    public string Porte { get; set; }
    public string Nome { get; set; }
    public string NomeFantasia { get; set; }
    public string Abertura { get; set; }
    public string NaturezaJuridica { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Efr { get; set; }
    public string Situacao { get; set; }
    public string DataSituacao { get; set; }
    public string MotivoSituacao { get; set; }
    public string SituacaoEspecial { get; set; }
    public string DataSituacaoEspecial { get; set; }
    public string CapitalSocial { get; set; }
    public string UsuarioId { get; set; }

    public EnderecoDto Endereco { get; set; }
    public List<AtividadePrincipalDto> AtividadesPrincipais { get; set; } = new();
    public List<AtividadeSecundariaDto> AtividadesSecundarias { get; set; } = new();
    public List<QsaDto> Qsa { get; set; } = new();
    public SimplesDto Simples { get; set; }
    public SimeiDto Simei { get; set; }
    public BillingDto Billing { get; set; }
}
