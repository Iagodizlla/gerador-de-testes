using System.Text.Json;

namespace TesteFacil.WebApp.Models;

public class NotificacaoViewModel
{
    public required string Titulo { get; set; }
    public required string Mensagem { get; set; }

    public static string GerarNotificacaoSerializada(string titulo, string mensagem)
    {
        var notificacao = new NotificacaoViewModel
        {
            Titulo = titulo,
            Mensagem = mensagem
        };

        var jsonString = JsonSerializer.Serialize(notificacao);

        return jsonString;
    }
}