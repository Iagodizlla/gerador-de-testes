using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_testesWebApp.Orm;

public static class DatabaseOperations
{
    public static void ApplyMigrations(this IHost app)
    {
        var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<GeradorDeTestesDbContext>();

        dbContext.Database.Migrate();
    }
}
