using Gerador_de_testes.Infraestrutura.Orm.ModuloDeTestes;
using Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;
using Gerador_de_testes.Infraestrutura.Orm.ModuloGestao;
using Gerador_de_testes.Infraestrutura.Orm.ModuloMateria;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testes.WebApp.DependencyInjection;
using Gerador_de_testes.WebApp.Orm;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Gerador_de_testes.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IRepositorioDisciplina, RepositorioDisciplinaEmOrm>();
        builder.Services.AddScoped<IRepositorioMateria, RepositorioMateriaEmOrm>();
        builder.Services.AddScoped<IRepositorioTeste, RepositorioTestesEmOrm>();
        builder.Services.AddScoped<IRepositorioQuestao, RepositorioQuestaoEmOrm>();

        builder.Services.AddEntityFrameworkConfig(builder.Configuration);
        builder.Services.AddSerilogConfig(builder.Logging);

        builder.Services.AddControllersWithViews();

        builder.Services.AddSingleton(typeof(IConverter),
            new SynchronizedConverter(new PdfTools()));

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