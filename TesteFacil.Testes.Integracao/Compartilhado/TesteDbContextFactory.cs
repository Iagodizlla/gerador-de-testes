using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Testes.Integracao.Compartilhado;

public class TesteDbContextFactory
{
    private readonly MsSqlContainer container;

    public TesteDbContextFactory()
    {
        container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithName("teste-facil-testdb-container")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InicializarAsync()
    {
        await container.StartAsync();
    }

    public async Task EncerrarAsync()
    {
        await container.StopAsync();
        await container.DisposeAsync();
    }

    public TesteFacilDbContext CriarDbContext()
    {
        var connectionString = string.Concat(container.GetConnectionString(), $";Initial Catalog=TesteFacilTestDb");

        var options = new DbContextOptionsBuilder<TesteFacilDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var dbContext = new TesteFacilDbContext(options);

        ConfigurarDbContext(dbContext);

        return dbContext;
    }

    private static void ConfigurarDbContext(TesteFacilDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Testes.RemoveRange(dbContext.Testes);
        dbContext.Questoes.RemoveRange(dbContext.Questoes);
        dbContext.Materias.RemoveRange(dbContext.Materias);
        dbContext.Disciplinas.RemoveRange(dbContext.Disciplinas);

        dbContext.SaveChanges();
    }
}