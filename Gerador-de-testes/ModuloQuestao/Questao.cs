using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloMateria;

namespace Gerador_de_testes.ModuloQuestao;

public class Questao : EntidadeBase<Questao>
{
    public string Enunciado{ get; set; }
    public Materia Materia { get; set; }
    public List<Alternativa> Alternativas { get; set; }

    public Questao()
    {
        Materia = new Materia();
        List<Alternativa> alternativas = new List<Alternativa>();
    }
    public Questao(string enunciado, Materia materia, List<Alternativa> alternativas) : this()
    {
        Id = Guid.NewGuid();
        Enunciado = enunciado;
        Alternativas = alternativas;
        Materia = materia;
    }
    public override void AtualizarRegistro(Questao registroEditado)
    {
        Enunciado = registroEditado.Enunciado;
        Materia = registroEditado.Materia;
        Alternativas = registroEditado.Alternativas;
    }
}
