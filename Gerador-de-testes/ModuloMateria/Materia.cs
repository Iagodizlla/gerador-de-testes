using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloQuestao;

namespace Gerador_de_testes.ModuloMateria;

public class Materia : EntidadeBase<Materia>
{
    public string Nome { get; set; }
    public string Serie { get; set; }
    public Disciplina Disciplina { get; set; }
    public List<Questao> Questoes { get; set; }

    public Materia()
    {
        Disciplina = new Disciplina();
    }
    public Materia(string nome, string serie, Disciplina disciplina, List<Questao> questoes) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
        Questoes = questoes;
    }

    public void RegistrarDisciplina(Disciplina disciplina)
    {
        this.RemoverDisciplina();
        Disciplina = disciplina;
        disciplina.Materias.Add(this);
    }
    public void RemoverDisciplina()
    {
        if (Disciplina != null)
        {
            Disciplina.Materias.Remove(this);
        }
    }
    public override void AtualizarRegistro(Materia registroEditado)
    {
        Nome = registroEditado.Nome;
        Serie = registroEditado.Serie;
        Disciplina = registroEditado.Disciplina;
    }
}