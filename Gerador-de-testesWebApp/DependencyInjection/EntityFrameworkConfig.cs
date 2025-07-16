namespace Gerador_de_testesWebApp.DependencyInjection
{
    public static class EntityFrameworkConfig
    {
        public static void AddEntityFrameworkConfig(
       this IServiceCollection services,
       IConfiguration configuration
   )
        {
            var connectionString = configuration["SQL_CONNECTION_STRING"];

            //remover comentado quando for usar o PostgreSQL
            //services.AddDbContext < gerador - de - testes > (options =>
            //    options.UseNpgsql(connectionString));
        }
    }
}
