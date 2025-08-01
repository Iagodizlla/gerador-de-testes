using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Testes.Integracao.Compartilhado;

public static class TesteDbContextFactory
{
    public static TesteFacilDbContext CriarDbContext()
    {
        var configuracao = CriarConfiguracao();

        var connectionString = configuracao["SQL_CONNECTION_STRING"];

        var options = new DbContextOptionsBuilder<TesteFacilDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var dbContext = new TesteFacilDbContext(options);

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }

    private static IConfiguration CriarConfiguracao()
    {
        var assembly = typeof(TesteDbContextFactory).Assembly;

        return new ConfigurationBuilder()
            .AddUserSecrets(assembly)
            .Build();
    }
}