using Gerador_de_testes.Infraestrutura.Orm.ModuloDeTestes;
using Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;
using Gerador_de_testes.Infraestrutura.Orm.ModuloGestao;
using Gerador_de_testes.Infraestrutura.Orm.ModuloMateria;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using TesteFacil.Aplicacao.ModuloDisciplina;
using TesteFacil.Aplicacao.ModuloMateria;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;
using TesteFacil.Infraestrutura.Orm.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.ModuloMateria;
using TesteFacil.Infraestrutura.Orm.ModuloQuestao;
using TesteFacil.Infraestrutura.Orm.ModuloTeste;
using TesteFacil.WebApp.ActionFilters;
using TesteFacil.WebApp.DependencyInjection;
using TesteFacil.WebApp.Orm;

namespace TesteFacil.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddScoped<DisciplinaAppService>();
            builder.Services.AddScoped<MateriaAppService>();
            builder.Services.AddScoped<IRepositorioDisciplina, RepositorioDisciplinaEmOrm>();
            builder.Services.AddScoped<IRepositorioMateria, RepositorioMateriaEmOrm>();
            builder.Services.AddScoped<IRepositorioQuestao, RepositorioQuestaoEmOrm>();
            builder.Services.AddScoped<IRepositorioTeste, RepositorioTesteEmOrm>();
            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
        }

        builder.Services.AddSerilogConfig(builder.Logging);

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ValidarModeloAttribute>();
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.ApplyMigrations();

            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/erro");
        }

        app.UseAntiforgery();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapDefaultControllerRoute();

        app.Run();
    }
}