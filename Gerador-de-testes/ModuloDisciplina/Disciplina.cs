using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloMateria;

namespace Gerador_de_testes.ModuloDisciplina;

public class Disciplina : EntidadeBase<Disciplina>
{
    public string Nome { get; set; }
    public List<Materia> Materias { get; set; }
    public Disciplina()
    {
        Materias = new List<Materia>();
    }
    public Disciplina(string nome) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }
    public override void AtualizarRegistro(Disciplina registroEditado)
    {
        Nome = registroEditado.Nome;
        Materias = registroEditado.Materias.Select(m => new Materia(m.Nome, m.Serie, m.Disciplina)).ToList();
    }
}