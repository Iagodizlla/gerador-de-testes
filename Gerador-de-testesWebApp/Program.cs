using Gerador_de_testesWebApp.DependencyInjection;
using Gerador_de_testesWebApp.Orm;
using Serilog;

namespace Gerador_de_testesWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddScoped<IRepositorio, RepositorioEmOrm>()

            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
            builder.Services.AddSerilogConfig(builder.Logging);

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.ApplyMigrations();

            app.UseAntiforgery();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
