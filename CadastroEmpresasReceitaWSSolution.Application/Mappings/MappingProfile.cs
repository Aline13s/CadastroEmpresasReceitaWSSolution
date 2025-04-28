using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Dtos;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using CadastroEmpresasReceitaWSSolution.Domain.ExternalModels;
using CadastroEmpresasReceitaWSSolution.Domain.ValueObjects;

namespace CadastroEmpresasReceitaWSSolution.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empresa, EmpresaDto>().ReverseMap();

            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<AtividadePrincipal, AtividadePrincipalDto>().ReverseMap();
            CreateMap<AtividadeSecundaria, AtividadeSecundariaDto>().ReverseMap();
            CreateMap<Qsa, QsaDto>().ReverseMap();
            CreateMap<Simples, SimplesDto>().ReverseMap();
            CreateMap<Simei, SimeiDto>().ReverseMap();
            CreateMap<Billing, BillingDto>().ReverseMap();

            CreateMap<ReceitaWsResponse, Empresa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => SomenteNumeros(src.Cnpj)))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
                .ForMember(dest => dest.Porte, opt => opt.MapFrom(src => src.Porte))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.NomeFantasia, opt => opt.MapFrom(src => src.Fantasia))
                .ForMember(dest => dest.Abertura, opt => opt.MapFrom(src => src.Abertura))
                .ForMember(dest => dest.NaturezaJuridica, opt => opt.MapFrom(src => src.NaturezaJuridica))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.Efr, opt => opt.MapFrom(src => src.Efr))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao))
                .ForMember(dest => dest.DataSituacao, opt => opt.MapFrom(src => src.DataSituacao))
                .ForMember(dest => dest.MotivoSituacao, opt => opt.MapFrom(src => src.MotivoSituacao))
                .ForMember(dest => dest.SituacaoEspecial, opt => opt.MapFrom(src => src.SituacaoEspecial))
                .ForMember(dest => dest.DataSituacaoEspecial, opt => opt.MapFrom(src => src.DataSituacaoEspecial))
                .ForMember(dest => dest.CapitalSocial, opt => opt.MapFrom(src => src.CapitalSocial))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => new Endereco
                {
                    Logradouro = src.Logradouro,
                    Numero = src.Numero,
                    Complemento = src.Complemento,
                    Bairro = src.Bairro,
                    Municipio = src.Municipio,
                    Uf = src.Uf,
                    Cep = src.Cep
                }))
                .ForMember(dest => dest.AtividadesPrincipais, opt => opt.MapFrom(src =>
                    src.AtividadePrincipal != null
                    ? src.AtividadePrincipal.Select(a => new AtividadePrincipal { Codigo = a.Code, Descricao = a.Text }).ToList()
                    : new List<AtividadePrincipal>()))
                .ForMember(dest => dest.AtividadesSecundarias, opt => opt.MapFrom(src =>
                    src.AtividadesSecundarias != null
                    ? src.AtividadesSecundarias.Select(a => new AtividadeSecundaria { Codigo = a.Code, Descricao = a.Text }).ToList()
                    : new List<AtividadeSecundaria>()))
                .ForMember(dest => dest.Qsa, opt => opt.MapFrom(src =>
                    src.Qsa != null
                    ? src.Qsa.Select(q => new Qsa { Nome = q.Nome, Qual = q.Qual, PaisOrigem = q.PaisOrigem, NomeRepLegal = q.NomeRepLegal, QualRepLegal = q.QualRepLegal }).ToList()
                    : new List<Qsa>()))
                .ForMember(dest => dest.Simples, opt => opt.MapFrom(src =>
                    src.Simples != null
                    ? new Simples
                    {
                        Optante = src.Simples.Optante,
                        DataOpcao = ParseDateTime(src.Simples.DataOpcao),
                        DataExclusao = ParseDateTime(src.Simples.DataExclusao),
                        UltimaAtualizacao = ParseDateTime(src.Simples.UltimaAtualizacao)
                    }
                    : null))
                .ForMember(dest => dest.Simei, opt => opt.MapFrom(src =>
                    src.Simei != null
                    ? new Simei
                    {
                        Optante = src.Simei.Optante,
                        DataOpcao = ParseDateTime(src.Simei.DataOpcao),
                        DataExclusao = ParseDateTime(src.Simei.DataExclusao),
                        UltimaAtualizacao = ParseDateTime(src.Simei.UltimaAtualizacao)
                    }
                    : null))
                .ForMember(dest => dest.Billing, opt => opt.MapFrom(src =>
                    src.Billing != null
                    ? new Billing { Free = src.Billing.Free, Database = src.Billing.Database }
                    : null));
        }

        private static DateTime? ParseDateTime(string? value)
        {
            return DateTime.TryParse(value, out var result) ? result : (DateTime?)null;
        }

        private static string SomenteNumeros(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return string.Empty;
            return new string(cnpj.Where(char.IsDigit).ToArray());
        }
    }
}
