using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TesteFacil.Dominio.Extensions;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Infraestrutura.Pdf;

public class TesteDocument : IDocument
{
    public Teste Model { get; }
    public bool Gabarito { get; }

    public TesteDocument(Teste model, bool gabarito = false)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        Model = model;
        Gabarito = gabarito;
    }

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Header().Height(50).Background("#0D6EFD").Element(ComposeHeader);
                page.Content().Background(Colors.Grey.Lighten3).Element(ComposeContent);
            });
    }

    void ComposeHeader(IContainer container)
    {
        container
            .PaddingHorizontal(30)
            .AlignLeft()
            .AlignMiddle()
            .Text("Teste Fácil").FontSize(15).Bold()
            .FontColor("#FFFFFF");
    }

    void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Element(ComposeTitle);
            column.Item().Element(ComposeTable);
            column.Item().Element(ComposeList);
        });
    }

    void ComposeTitle(IContainer container)
    {
        container.PaddingHorizontal(30).PaddingTop(20).Row(row =>
        {
            row
                .RelativeItem()
                .Background(Colors.Grey.Lighten3)
                .Text(Model.Titulo).FontSize(13).Bold();
        });
    }

    void ComposeTable(IContainer container)
    {
        container.PaddingHorizontal(30).PaddingVertical(20).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.ConstantColumn(70);
            });

            table.Header(header =>
            {
                header.Cell().BorderBottom(2).PaddingVertical(8).Text("Disciplina").Bold();
                header.Cell().BorderBottom(2).PaddingVertical(8).Text("Matéria").Bold();
                header.Cell().BorderBottom(2).PaddingVertical(8).Text("Série").Bold();
                header.Cell().BorderBottom(2).PaddingVertical(8).AlignRight().Text("Questões").Bold();
            });

            table.Cell().PaddingVertical(8).Text(Model.Disciplina.Nome);
            table.Cell().PaddingVertical(8).Text(Model.Materia?.Nome ?? "Recuperação");
            table.Cell().PaddingVertical(8).Text(Model.Serie.GetDisplayName());
            table.Cell().PaddingVertical(8).AlignRight().Text(Model.QuantidadeQuestoes.ToString());
        });
    }

    void ComposeList(IContainer container)
    {
        container.PaddingHorizontal(30).PaddingTop(20).Column(col =>
        {
            col.Spacing(10);

            for (int i = 0; i < Model.Questoes.Count; i++)
            {
                var questao = Model.Questoes[i];

                col.Item().PaddingVertical(10).Row(row =>
                {
                    row.ConstantItem(20).Text($"{i + 1}.").Bold();
                    row.RelativeItem().Text(questao.Enunciado).Bold();
                });

                var alternativasOrdenadas = Model.Questoes[i].Alternativas
                    .OrderBy(a => a.Letra)
                    .ToList();

                for (int j = 0; j < alternativasOrdenadas.Count; j++)
                {
                    var alternativa = alternativasOrdenadas[j];

                    col.Item().PaddingLeft(30).Row(row =>
                    {
                        if (Gabarito && alternativa.Correta)
                        {
                            row.ConstantItem(20).Text($"{alternativa.Letra})").FontColor(Colors.Green.Medium);
                            row.RelativeItem().Text(alternativa.Resposta).FontColor(Colors.Green.Medium);
                        }
                        else
                        {
                            row.ConstantItem(20).Text($"{alternativa.Letra})");
                            row.RelativeItem().Text(alternativa.Resposta);
                        }
                    });
                }

            }
        });
    }
}