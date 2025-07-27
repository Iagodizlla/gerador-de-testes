namespace TesteFacil.Dominio.ModuloQuestao;

public class Alternativa
{
    public Guid Id { get; set; }
    public char Letra { get; set; }
    public string Resposta { get; set; }
    public bool Correta { get; set; }
    public Questao Questao { get; set; }

    protected Alternativa() { }

    public Alternativa(char letra, string resposta, bool correta, Questao questao) : this()
    {
        Id = Guid.NewGuid();
        Letra = letra;
        Resposta = resposta;
        Questao = questao;
        Correta = correta;
    }
}