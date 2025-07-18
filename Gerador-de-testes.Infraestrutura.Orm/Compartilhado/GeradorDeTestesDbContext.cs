using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_testes.Infraestrutura.Orm.Compartilhado;

public class GeradorDeTestesDbContext : DbContext
{
    public DbSet<Disciplina> Disciplinas { get; set; }
    public DbSet<Materia> Materias { get; set; }
    public DbSet<Teste> Testes { get; set; }


    public GeradorDeTestesDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(GeradorDeTestesDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}