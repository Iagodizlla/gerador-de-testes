using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Testes.Integracao.Compartilhado;

public class TesteFacilDbContextFactory
{
    private readonly MsSqlContainer container;

    public TesteFacilDbContextFactory()
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

        return dbContext;
    }
}