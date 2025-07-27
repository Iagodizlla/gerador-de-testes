using Microsoft.EntityFrameworkCore;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.WebApp.Orm;

public static class DatabaseOperations
{
    public static void ApplyMigrations(this IHost app)
    {
        var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TesteFacilDbContext>();

        dbContext.Database.Migrate();
    }
}