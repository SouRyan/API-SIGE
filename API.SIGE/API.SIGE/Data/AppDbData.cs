using API.SIGE.Interfaces;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Data;

public class AppDbData : DbContext
{
    private readonly ITenantProvider? _tenantProvider;

    public AppDbData(DbContextOptions<AppDbData> options, ITenantProvider? tenantProvider = null)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<Caixilho> Caixilhos => Set<Caixilho>();
    public DbSet<Obra> Obras => Set<Obra>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<FamiliaCaixilho> FamiliaCaixilhos => Set<FamiliaCaixilho>();
    public DbSet<Cargo> Cargos => Set<Cargo>();
    public DbSet<Medicao> Medicoes => Set<Medicao>();
    public DbSet<ProducaoFamilia> ProducoesFamilia => Set<ProducaoFamilia>();
    public DbSet<Anexo> Anexos => Set<Anexo>();
    public DbSet<Notificacao> Notificacoes => Set<Notificacao>();
    public DbSet<SolicitacaoCliente> SolicitacoesCliente => Set<SolicitacaoCliente>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<SolicitacaoCadastro> SolicitacoesCadastro => Set<SolicitacaoCadastro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- Multi-tenant: Global Query Filters ---
        modelBuilder.Entity<Usuario>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<Obra>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<Caixilho>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<FamiliaCaixilho>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<Cargo>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<Medicao>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<ProducaoFamilia>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<Anexo>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<Notificacao>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<SolicitacaoCliente>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());
        modelBuilder.Entity<TipoUsuario>().HasQueryFilter(e => _tenantProvider == null || !_tenantProvider.HasTenant() || e.IdEmpresa == _tenantProvider.GetTenantId());

        // --- Relationships ---
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasOne(u => u.TipoUsuario)
                .WithMany()
                .HasForeignKey("IdTipoUsuario")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Cargo)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.IdCargo)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(u => u.Empresa)
                .WithMany()
                .HasForeignKey(u => u.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(o => o.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.Cliente)
                .WithMany()
                .HasForeignKey(o => o.IdCliente)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(o => o.Empresa)
                .WithMany()
                .HasForeignKey(o => o.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FamiliaCaixilho>(entity =>
        {
            entity.HasOne(f => f.Obra)
                .WithMany(o => o.FamiliasCaixilho!)
                .HasForeignKey(f => f.IdObra)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasIndex(c => new { c.TipoCargo, c.IdEmpresa }).IsUnique();
        });

        modelBuilder.Entity<Medicao>(entity =>
        {
            entity.HasOne(m => m.FamiliaCaixilho)
                .WithMany()
                .HasForeignKey(m => m.IdFamiliaCaixilho)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Responsavel)
                .WithMany()
                .HasForeignKey(m => m.IdResponsavel)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProducaoFamilia>(entity =>
        {
            entity.HasOne(p => p.FamiliaCaixilho)
                .WithMany()
                .HasForeignKey(p => p.IdFamiliaCaixilho)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Responsavel)
                .WithMany()
                .HasForeignKey(p => p.IdResponsavel)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Anexo>(entity =>
        {
            entity.HasOne(a => a.Medicao)
                .WithMany()
                .HasForeignKey(a => a.IdMedicao)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.ProducaoFamilia)
                .WithMany()
                .HasForeignKey(a => a.IdProducaoFamilia)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Notificacao>(entity =>
        {
            entity.HasOne(n => n.UsuarioDestino)
                .WithMany()
                .HasForeignKey(n => n.IdUsuarioDestino)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(n => n.Obra)
                .WithMany()
                .HasForeignKey(n => n.IdObra)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SolicitacaoCliente>(entity =>
        {
            entity.HasOne(s => s.Caixilho)
                .WithMany()
                .HasForeignKey(s => s.IdCaixilho)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Cliente)
                .WithMany()
                .HasForeignKey(s => s.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
