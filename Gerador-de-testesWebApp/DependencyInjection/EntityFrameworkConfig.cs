using Microsoft.EntityFrameworkCore;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.WebApp.DependencyInjection;

public static class EntityFrameworkConfig
{
    public static void AddEntityFrameworkConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration["SQL_CONNECTION_STRING"];

        services.AddDbContext<IUnitOfWork, TesteFacilDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}