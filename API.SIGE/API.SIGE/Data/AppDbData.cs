using API.SIGE.Models;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Models;

namespace API.SIGE.Data;

public class AppDbData : DbContext
{
    public AppDbData(DbContextOptions<AppDbData> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("WebApiDatabase");
        optionsBuilder.UseNpgsql(connectionString);
    }

    public DbSet<Caixilho> Caixilhos => Set<Caixilho>();
    public DbSet<Obra> Obras => Set<Obra>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<FamiliaCaixilho> FamiliaCaixilhos => Set<FamiliaCaixilho>();
    //public DbSet<TipoCaixilho> TiposCaixilho => Set<TipoCaixilho>();
    public DbSet<Producao> Producoes => Set<Producao>();
    public DbSet<RelatorioProducao> RelatoriosProducao => Set<RelatorioProducao>();

 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Modelo Usuario não expõe IdTipoUsuario; coluna necessária para o relacionamento com TipoUsuario.
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasOne(u => u.TipoUsuario)
                .WithMany()
                .HasForeignKey("IdTipoUsuario")
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
