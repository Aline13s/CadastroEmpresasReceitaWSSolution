using CadastroEmpresasReceitaWSSolution.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CadastroEmpresasReceitaWSSolution.Infrastructure.Identity
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Empresa>(empresa =>
            {
                empresa.HasOne<Usuario>()
                       .WithMany()
                       .HasForeignKey(e => e.UsuarioId);

                empresa.OwnsOne(e => e.Endereco, endereco =>
                {
                    endereco.Property(e => e.Logradouro).HasMaxLength(100);
                    endereco.Property(e => e.Numero).HasMaxLength(20);
                    endereco.Property(e => e.Complemento).HasMaxLength(100);
                    endereco.Property(e => e.Bairro).HasMaxLength(60);
                    endereco.Property(e => e.Municipio).HasMaxLength(60);
                    endereco.Property(e => e.Uf).HasMaxLength(2);
                    endereco.Property(e => e.Cep).HasMaxLength(10);
                });

                empresa.OwnsOne(e => e.Simples, simples =>
                {
                    simples.Property(s => s.Optante).HasColumnName("Simples_Optante");
                    simples.Property(s => s.DataOpcao).HasColumnName("Simples_DataOpcao");
                    simples.Property(s => s.DataExclusao).HasColumnName("Simples_DataExclusao");
                    simples.Property(s => s.UltimaAtualizacao).HasColumnName("Simples_UltimaAtualizacao");
                });

                empresa.OwnsOne(e => e.Simei, simei =>
                {
                    simei.Property(s => s.Optante).HasColumnName("Simei_Optante");
                    simei.Property(s => s.DataOpcao).HasColumnName("Simei_DataOpcao");
                    simei.Property(s => s.DataExclusao).HasColumnName("Simei_DataExclusao");
                    simei.Property(s => s.UltimaAtualizacao).HasColumnName("Simei_UltimaAtualizacao");
                });

                empresa.OwnsOne(e => e.Billing, billing =>
                {
                    billing.Property(b => b.Free).HasColumnName("Billing_Free");
                    billing.Property(b => b.Database).HasColumnName("Billing_Database");
                });

                empresa.OwnsMany(e => e.AtividadesPrincipais, atividades =>
                {
                    atividades.WithOwner().HasForeignKey("EmpresaId");
                    atividades.Property<Guid>("Id");
                    atividades.HasKey("Id");
                    atividades.Property(a => a.Codigo).HasMaxLength(20);
                    atividades.Property(a => a.Descricao).HasMaxLength(255);
                });

                empresa.OwnsMany(e => e.AtividadesSecundarias, atividades =>
                {
                    atividades.WithOwner().HasForeignKey("EmpresaId");
                    atividades.Property<Guid>("Id");
                    atividades.HasKey("Id");
                    atividades.Property(a => a.Codigo).HasMaxLength(20);
                    atividades.Property(a => a.Descricao).HasMaxLength(255);
                });

                empresa.OwnsMany(e => e.Qsa, qsa =>
                {
                    qsa.WithOwner().HasForeignKey("EmpresaId");
                    qsa.Property<Guid>("Id");
                    qsa.HasKey("Id");
                    qsa.Property(q => q.Nome).HasMaxLength(255);
                    qsa.Property(q => q.Qual).HasMaxLength(100);
                    qsa.Property(q => q.PaisOrigem).HasMaxLength(100);
                    qsa.Property(q => q.NomeRepLegal).HasMaxLength(255);
                    qsa.Property(q => q.QualRepLegal).HasMaxLength(100);
                });
            });
        }
    }
}