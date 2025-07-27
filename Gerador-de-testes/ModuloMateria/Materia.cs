using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Dominio.ModuloMateria;

public class Materia : EntidadeBase<Materia>
{
    public string Nome { get; set; }
    public SerieMateria Serie { get; set; }
    public Disciplina Disciplina { get; set; }
    public List<Questao> Questoes { get; set; }
    public List<Teste> Testes { get; set; }

    protected Materia()
    {
        Questoes = new List<Questao>();
        Testes = new List<Teste>();
    }

    public Materia(string nome, SerieMateria serie, Disciplina disciplina) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
    }

    public void AdicionarQuestao(Questao questao)
    {
        if (Questoes.Contains(questao))
            return;

        Questoes.Add(questao);
    }

    public void RemoverQuestao(Questao questao)
    {
        if (!Questoes.Contains(questao))
            return;

        Questoes.Remove(questao);
    }

    public List<Questao> ObterQuestoesAleatorias(int quantidadeQuestoes)
    {
        var random = new Random();

        return Questoes
            .OrderBy(q => random.Next())
            .Take(quantidadeQuestoes)
            .ToList();
    }

    public override void AtualizarRegistro(Materia registroEditado)
    {
        Nome = registroEditado.Nome;
        Serie = registroEditado.Serie;
        Disciplina = registroEditado.Disciplina;
    }
}