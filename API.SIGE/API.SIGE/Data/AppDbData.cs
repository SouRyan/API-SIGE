using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Data;

public class AppDbData : DbContext
{
    public AppDbData(DbContextOptions<AppDbData> options)
        : base(options)
    {
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(o => o.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
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
            entity.HasIndex(c => c.TipoCargo).IsUnique();

            entity.HasData(
                new Cargo { IdCargo = 1, TipoCargo = TipoCargo.Gerente, DescricaoCargo = "Gerente" },
                new Cargo { IdCargo = 2, TipoCargo = TipoCargo.ResponsavelVerificacao, DescricaoCargo = "Responsável pela verificação" },
                new Cargo { IdCargo = 3, TipoCargo = TipoCargo.ResponsavelMedicao, DescricaoCargo = "Responsável pela medição" },
                new Cargo { IdCargo = 4, TipoCargo = TipoCargo.ResponsavelProducao, DescricaoCargo = "Responsável pela produção" });
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
    }
}
