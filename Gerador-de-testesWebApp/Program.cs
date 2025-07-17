using Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.WebApp.DependencyInjection;
using Gerador_de_testes.WebApp.Orm;

namespace Gerador_de_testes.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IRepositorioDisciplina, RepositorioDisciplinaEmOrm>();

        builder.Services.AddEntityFrameworkConfig(builder.Configuration);
        builder.Services.AddSerilogConfig(builder.Logging);

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.ApplyMigrations();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/erro");
        else
            app.UseDeveloperExceptionPage();

        app.UseAntiforgery();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapDefaultControllerRoute();

        app.Run();
    }
}