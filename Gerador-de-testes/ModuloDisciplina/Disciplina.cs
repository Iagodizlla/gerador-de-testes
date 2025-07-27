using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Dominio.ModuloDisciplina;

public class Disciplina : EntidadeBase<Disciplina>
{
    public string Nome { get; set; }
    public List<Materia> Materias { get; set; }
    public List<Teste> Testes { get; set; }

    protected Disciplina()
    {
        Materias = new List<Materia>();
        Testes = new List<Teste>();
    }

    public Disciplina(string nome) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public List<Questao> ObterQuestoesAleatorias(int quantidadeQuestoes, SerieMateria serie)
    {
        var questoesRelacionadas = new List<Questao>();

        foreach (var mat in Materias)
        {
            if (mat.Serie.Equals(serie))
                questoesRelacionadas.AddRange(mat.Questoes);
        }

        var random = new Random();

        return questoesRelacionadas
            .OrderBy(q => random.Next())
            .Take(quantidadeQuestoes)
            .ToList();
    }

    public override void AtualizarRegistro(Disciplina registroEditado)
    {
        Nome = registroEditado.Nome;
    }
}