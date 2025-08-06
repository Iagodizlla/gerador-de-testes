using Microsoft.EntityFrameworkCore;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Testes.Integracao.Compartilhado;

public static class TesteFacilDbContextFactory
{
    public static TesteFacilDbContext CriarDbContext(string connectionString)
    {

        var options = new DbContextOptionsBuilder<TesteFacilDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var dbContext = new TesteFacilDbContext(options);

        return dbContext;
    }
}