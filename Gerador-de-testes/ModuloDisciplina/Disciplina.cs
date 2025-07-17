using Gerador_de_testes.Compartilhado;

namespace Gerador_de_testes.ModuloDisciplina;

public class Disciplina : EntidadeBase<Disciplina>
{
    public string Nome { get; set; }

    public Disciplina()
    {
    }
    public Disciplina(string nome) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public override void AtualizarRegistro(Disciplina registroEditado)
    {
        Nome = registroEditado.Nome;
    }
}