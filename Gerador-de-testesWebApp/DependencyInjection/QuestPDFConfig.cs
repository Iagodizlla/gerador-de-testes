using TesteFacil.Dominio.ModuloTeste;
using TesteFacil.Infraestrutura.Pdf;

namespace TesteFacil.WebApp.DependencyInjection;

public static class QuestPDFConfig
{
    public static void AddQuestPDFConfig(this IServiceCollection services)
    {
        services.AddScoped<IGeradorTeste, GeradorTesteEmPdf>();
    }
}