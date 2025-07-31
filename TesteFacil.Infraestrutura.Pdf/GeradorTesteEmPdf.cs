using QuestPDF.Fluent;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Infraestrutura.Pdf;

public class GeradorTesteEmPdf : IGeradorTeste
{
    public byte[] GerarNovoTeste(Teste teste, bool gabarito)
    {
        var documento = new TesteDocument(teste, gabarito);

        var pdfBytes = documento.GeneratePdf();

        return pdfBytes;
    }
}