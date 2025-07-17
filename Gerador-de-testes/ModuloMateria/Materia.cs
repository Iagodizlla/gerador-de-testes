using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.ModuloMateria;

public class Materia : EntidadeBase<Materia>
{
    public string Nome { get; set; }
    public string Serie { get; set; }
    public Disciplina Disciplina { get; set; }

    public Materia()
    {
        Disciplina = new Disciplina();
    }
    public Materia(string nome, string serie, Disciplina disciplina) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
    }
    public override void AtualizarRegistro(Materia registroEditado)
    {
        Nome = registroEditado.Nome;
        Serie = registroEditado.Serie;
        Disciplina = registroEditado.Disciplina;
    }
}