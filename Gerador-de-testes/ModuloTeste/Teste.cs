using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;

namespace TesteFacil.Dominio.ModuloTeste;

public class Teste : EntidadeBase<Teste>
{
    public DateTime DataGeracao { get; set; }
    public string Titulo { get; set; }
    public bool Recuperacao { get; set; }
    public int QuantidadeQuestoes { get; set; }
    public Disciplina Disciplina { get; set; }
    public Materia? Materia { get; set; }
    public SerieMateria Serie { get; set; }
    public List<Questao> Questoes { get; set; } = new List<Questao>();

    public Teste() { }

    public Teste(
        string titulo,
        bool recuperacao,
        int quantidadeQuestoes,
        SerieMateria serieMateria,
        Disciplina disciplina,
        Materia? materia
    ) : this()
    {
        Id = Guid.NewGuid();
        DataGeracao = DateTime.UtcNow;

        Titulo = titulo;
        Recuperacao = recuperacao;
        QuantidadeQuestoes = quantidadeQuestoes;
        Serie = serieMateria;
        Disciplina = disciplina;
        Materia = materia;
    }

    public Teste(
        string titulo,
        bool recuperacao,
        int quantidadeQuestoes,
        SerieMateria serieMateria,
        Disciplina disciplina,
        Materia? materia,
        List<Questao> questoes
    ) : this()
    {
        Id = Guid.NewGuid();
        DataGeracao = DateTime.UtcNow;

        Titulo = titulo;
        Recuperacao = recuperacao;
        QuantidadeQuestoes = quantidadeQuestoes;
        Serie = serieMateria;
        Disciplina = disciplina;
        Materia = materia;

        foreach (var questao in questoes)
            AdicionarQuestao(questao);
    }

    public List<Questao>? SortearQuestoes()
    {
        RemoverQuestoesAtuais();

        var questoesSorteadas = new List<Questao>(QuantidadeQuestoes);

        if (Recuperacao)
            questoesSorteadas = Disciplina.ObterQuestoesAleatorias(QuantidadeQuestoes, Serie);
        else
            questoesSorteadas = Materia?.ObterQuestoesAleatorias(QuantidadeQuestoes);

        if (questoesSorteadas is not null)
        {
            foreach (Questao q in questoesSorteadas)
                AdicionarQuestao(q);
        }

        return questoesSorteadas;
    }

    public void AdicionarQuestao(Questao questao)
    {
        questao.UtilizadaEmTeste = true;

        Questoes.Add(questao);
    }

    public void RemoverQuestao(Questao questao)
    {
        questao.UtilizadaEmTeste = false;

        Questoes.Remove(questao);
    }

    public void RemoverQuestoesAtuais()
    {
        foreach (var questao in Questoes)
            RemoverQuestao(questao);
    }

    public override void AtualizarRegistro(Teste registroEditado)
    {
        Titulo = registroEditado.Titulo;
        Disciplina = registroEditado.Disciplina;
        Materia = registroEditado.Materia;
        Questoes = registroEditado.Questoes;
        Recuperacao = registroEditado.Recuperacao;
    }
}