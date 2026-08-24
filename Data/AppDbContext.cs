using Microsoft.EntityFrameworkCore;
using VeiculosApi.Models;

namespace VeiculosApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Veiculo>()
            .HasOne(v => v.Marca)
            .WithMany(m => m.Veiculos)
            .HasForeignKey(v => v.MarcaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed inicial de marcas para facilitar os testes
        modelBuilder.Entity<Marca>().HasData(
            new Marca { Id = 1, Nome = "Volkswagen", Ativo = true },
            new Marca { Id = 2, Nome = "Fiat", Ativo = true },
            new Marca { Id = 3, Nome = "Chevrolet", Ativo = false }
        );
    }
}
