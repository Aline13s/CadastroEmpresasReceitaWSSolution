using Microsoft.AspNetCore.Identity;
using CadastroEmpresasReceitaWSSolution.Domain.Entities;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Identity;

public class Usuario : IdentityUser
{
    public ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();
}